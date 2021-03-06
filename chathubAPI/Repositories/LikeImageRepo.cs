﻿using chathubAPI.DATA;
using chathubAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chathubAPI.Repositories
{
    public class LikeImageRepo : ILikeImageRepo
    {
        readonly ApplicationDbContext _dbContext;
        public LikeImageRepo(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool LikeImage(string imageId, string userId)
        {
            if (userId != null)
            {
                try
                {
                    if(CheckIfLiked(imageId,userId) == null)
                    {
                        LikedImage likedImage = new LikedImage
                        {
                            ImageId = imageId,
                            LikedById = userId
                        };

                        _dbContext.LikedImages.AddAsync(likedImage);
                        _dbContext.SaveChangesAsync();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            return false;
        }

        public bool DeleteLikeImage (LikedImage likedImage)
        {
            try
            {
                _dbContext.LikedImages.Remove(likedImage);
                _dbContext.SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                throw new Exception();
            }
        }

        public LikedImage CheckIfLiked(string imageId, string userId)
        {

            try
            {
                return _dbContext.LikedImages.Where(x => x.ImageId == imageId && x.LikedById == userId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
        }

        public int CountImageLikes(string imageId)
        {
            try
            {
                return _dbContext.LikedImages.Where(x => x.ImageId == imageId).Count();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Save()
        {
            _dbContext.SaveChangesAsync();
        }
    }
}
