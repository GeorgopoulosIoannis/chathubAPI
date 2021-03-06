﻿using chathubAPI.DATA;
using chathubAPI.DTO;
using chathubAPI.Helpers;
using chathubAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chathubAPI.Repositories
{
    public class ProfileRepo : IProfileRepo
    {
        readonly ApplicationDbContext _dbContext;
        public ProfileRepo(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public bool Add(User user)
        {
            Profile profile = new Profile
            {
                UserId = user.Id,
                Alias = user.Email.Split('@').First(),
                User = user,
                Avatar = "",
                Description = "No description yet.."
            };
            try
            {

                _dbContext.AddAsync(profile);
                _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Profile Get(string userId)
        {
            try
            {
                return _dbContext.Profiles.Where(x => x.UserId == userId).Include(x=>x.Images).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool Update(Profile prof)
        {
            try
            {
                Profile oldProf = _dbContext.Profiles.Where(x => x.UserId == prof.UserId).FirstOrDefault();
                if (!String.IsNullOrWhiteSpace(prof.Alias))
                {
                    oldProf.Alias = prof.Alias;
                }
                if (!String.IsNullOrWhiteSpace(prof.Avatar))
                {
                    oldProf.Avatar = prof.Avatar;
                }
                if (!String.IsNullOrWhiteSpace(prof.Description))
                {
                    oldProf.Description = prof.Description;
                }
                _dbContext.Update(oldProf);
                _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<Profile> GetRandomProfiles(string userId, int currentPage = 1)
        {
            int start = (currentPage - 1) * Constants.PROFILES_PER_PAGE;
            return _dbContext.Profiles.Where(x => x.UserId != userId).Skip(start).Take(Constants.PROFILES_PER_PAGE).ToList();
        }

        public void Save()
        {
            _dbContext.SaveChangesAsync();
        }
    }
}

