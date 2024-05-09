//using motorcycleGestionCSharp.domain;

using System;
using System.Collections;
using System.Collections.Generic;
using DomainModule.domain;
using RepoModule.Repo;


namespace ServiceModule.Service 
{

    public class RaceService
    {
        private RaceRepo raceRepo;

        public RaceService(RaceRepo raceRepo)
        {
            this.raceRepo = raceRepo;
        }

        public List<Race> ShowRacesByEngineSize(EngineCapacity engCap)
        {
            return null;
        }

        public int? ShowParticipantNumberByRaceName(string name)
        {
            return null;
        }

        public int? GetParticipantSize()
        {
            return null;
        }

        public void add(Race race1)
        {
            raceRepo.Add(race1);
        }

        public int getLasId()
        {
            return raceRepo.GetLastID();
        }
        public List<Race> getAll()
        {
            return raceRepo.GetAll();
        }

        public List<Race>getAllByEngCap(string input)
        {
            List<Race> races = new List<Race>();
            foreach (var VARIABLE in getAll())
            {    
                if (VARIABLE.EngineCap.ToString().CompareTo(input) == 0)
                {
                    races.Add(VARIABLE);
                }
            }
            return races;
        }
    }
}