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
            Converters = { new DateOnlyJsonConverter() } // Din tidligere converter
        };

        public FileScheduleRepository(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("filePath må ikke være tomt", nameof(filePath));

            _filePath = filePath;
            Directory.CreateDirectory(Path.GetDirectoryName(_filePath)!);
            Debug.WriteLine($"[Repo] showschedules.json path: {_filePath}");
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
            var schedules = GetAll().ToList();
            schedules.Add(schedule);
            SaveAll(schedules);
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