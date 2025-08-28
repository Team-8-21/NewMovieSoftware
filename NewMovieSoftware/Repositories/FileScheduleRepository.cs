using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using MovieOrganiser2000.Helpers;
using MovieOrganiser2000.Models;
using Newtonsoft.Json;
using ScheduleOrganiser2000.Repositories;

namespace MovieOrganiser2000.Repositories
{
    public class FileScheduleRepository : IScheduleRepository
    {
        private readonly string _filePath;

        private static readonly JsonSerializerSettings _jsonSettings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            Converters = { new DateOnlyJsonConverter(), new TimeOnlyJsonConverter() } // Din tidligere converter
        };



        public FileScheduleRepository(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("filePath må ikke være tomt", nameof(filePath));

            _filePath = filePath;
            Directory.CreateDirectory(Path.GetDirectoryName(_filePath)!);
            Debug.WriteLine($"[Repo] showschedules.json path: {_filePath}");

            // DEBUG: Print the exact file path
            Debug.WriteLine($"[Repo] showschedules.json FULL path: {Path.GetFullPath(_filePath)}");
            Debug.WriteLine($"[Repo] File exists: {File.Exists(_filePath)}");
            if (File.Exists(_filePath))
            {
                Debug.WriteLine($"[Repo] File size: {new FileInfo(_filePath).Length} bytes");
            }
        }

        public IEnumerable<Schedule> GetAll()
        {
            if (!File.Exists(_filePath))
                return new List<Schedule>();

            var json = File.ReadAllText(_filePath);
            return JsonConvert.DeserializeObject<List<Schedule>>(json, _jsonSettings) ?? new List<Schedule>();
        }

        public Schedule GetSchedule(Schedule schedule)
        {
            return GetAll().FirstOrDefault(m => m.ScheduleId == schedule.ScheduleId);
        }

        private void SaveAll(IEnumerable<Schedule> schedules)
        {
            var json = JsonConvert.SerializeObject(schedules, _jsonSettings);
            File.WriteAllText(_filePath, json);
        }

        IEnumerable<Schedule> IScheduleRepository.GetAll()
        {
            return GetAll();
        }

        Schedule IScheduleRepository.GetSchedule(Schedule schedule)
        {
            return GetSchedule(schedule);
        }

        void IScheduleRepository.AddSchedule(Schedule schedule)
        {
            /*
            var schedules = GetAll().ToList();
            schedules.Add(schedule);
            SaveAll(schedules);
            */

            //DEBUG
            Debug.WriteLine($"[Repo] AddSchedule called with schedule ID: {schedule.ScheduleId}");
            var schedules = GetAll().ToList();
            Debug.WriteLine($"[Repo] Existing schedules count: {schedules.Count}");

            schedules.Add(schedule);
            Debug.WriteLine($"[Repo] After add, schedules count: {schedules.Count}");

            SaveAll(schedules);
            Debug.WriteLine($"[Repo] SaveAll completed");

            // Verify file was written
            if (File.Exists(_filePath))
            {
                var content = File.ReadAllText(_filePath);
                Debug.WriteLine($"[Repo] File content after save: {content.Substring(0, Math.Min(200, content.Length))}...");
            }
        }

        void IScheduleRepository.UpdateSchedule(Schedule schedule)
        {
            var schedules = GetAll().ToList();
            var idx = schedules.FindIndex(m => m.ScheduleId == schedule.ScheduleId);
            if (idx == -1) throw new KeyNotFoundException("Fremvisning ikke fundet");
            schedules[idx] = schedule;
            SaveAll(schedules);
        }

        void IScheduleRepository.DeleteSchedule(Schedule schedule)
        {
            var schedules = GetAll().ToList();
            schedules.RemoveAll(m => m.ScheduleId == schedule.ScheduleId);
            SaveAll(schedules);
        }
    }
}