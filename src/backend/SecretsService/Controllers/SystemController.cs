﻿using System;
using Keebox.Common.Managers;
using Keebox.Common.Types;
using Keebox.SecretsService.Models;
using Microsoft.AspNetCore.Mvc;


namespace Keebox.SecretsService.Controllers
{
    [ApiController]
    [Route("system")]
    public class SystemController : ControllerBase
    {
        public SystemController(Configuration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("status")]
        [ProducesResponseType(200)]
        public SystemInfo GetSystemInfo()
        {
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            var fileVersionInfo = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
            var version = fileVersionInfo.ProductVersion ?? throw new InvalidOperationException();
            return new SystemInfo
            {
                Version = version,
                StorageType = _configuration.Engine.ToString()!
            };
        }

        [HttpGet("config")]
        [ProducesResponseType(200)]
        public Config GetConfig()
        {
            // TODO: add real config
            return new Config();
        }

        [HttpGet("start")]
        [ProducesResponseType(200)]
        public void Start()
        {
            // TODO: add start logic
        }

        [HttpGet("stop")]
        [ProducesResponseType(200)]
        public void Stop()
        {
            // TODO: add stop logic
        }

        private readonly Configuration _configuration;
    }
}