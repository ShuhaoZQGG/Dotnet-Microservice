﻿using PlatformService.Models;

namespace PlatformService.Data
{
    public class PlatformRepo: IPlatformRepo
    {
        public bool SaveChanges()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Platform> GetAllPlatforms()
        {
            throw new NotImplementedException();
        }

        public Platform GetPlatformById(int id)
        {
            throw new NotImplementedException();
        }

        public void CreatePlatform(Platform platform)
        {
            throw new NotImplementedException();
        }
    }
}
