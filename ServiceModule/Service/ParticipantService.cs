using System;
using System.Collections;
using System.Collections.Generic;
using DomainModule.domain;
using RepoModule.Repo;


namespace ServiceModule.Service
{
    public class ParticipantService
    {
        private ParticipantRepo participantRepo;
        private RaceRepo raceRepo;

        public ParticipantService(ParticipantRepo participantRepo, RaceRepo raceRepo)
        {
            this.participantRepo = participantRepo;
            this.raceRepo = raceRepo;
        }

        public void add(Participant participant)
        {
            int idRace = participant.IdRace;
            int id = participant.Id;
            string teamName = participant.TeamName;
            string name = participant.Name;
            participantRepo.registerParticipant(id,name,teamName,idRace);
        }

        public void SearchByTeamName(string teamName)
        {
            // Implementation for searching participants by team name
        }

        public int GetLasIDParticipant()
        {
            return participantRepo.GetLastID();
        }

        public List<Participant> GetAllParticipants()
        {
            try
            {
                return participantRepo.GetAll();
            }
            catch (RepositoryException ex)
            {
                // Handle the exception (e.g., log or rethrow)
                throw;
            }
        }

        public void RegisterParticipantToRace(int idParticipant,String name,String teamName ,int idRace)
        {
            participantRepo.registerParticipant(idParticipant, name,teamName, idRace);
        }

        public int getNrParticipantsAtRace(int idRace)
        {
            int count = 0;
            List<Participant> participants = participantRepo.GetAll();
            foreach (var VARIABLE in participants)
            {
                if (VARIABLE.IdRace == idRace)
                {
                    count += 1;
                }
            }

            return count;
        }

        public List<Participant> getAllByTeamName(string searchTxtText)
        {
            List<Participant> participants = new List<Participant>();
            foreach (var VARIABLE in GetAllParticipants())
            {
                if (VARIABLE.TeamName==searchTxtText)
                {
                    participants.Add(VARIABLE);
                }
            }

            return participants;
        }
    }
}