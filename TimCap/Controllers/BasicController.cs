﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using TimCap.DAO;
using TimCap.Model;

namespace TimCap.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasicController : ControllerBase
    {
        private readonly TimeCapContext _context;
        private IMemoryCache _cache;
        private MemoryCacheEntryOptions _options;

        public BasicController(TimeCapContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
            _options = new MemoryCacheEntryOptions()
            {
                SlidingExpiration = TimeSpan.FromMinutes(10),
            };
        }


        [HttpGet("test")]
        public string Func()
        {
            return "ok";
        }

        [HttpPost("timecap/loginccnu")]
        public ApiRes LoginCcnu([Required] string UserId,[Required] string pwd,[Required] string session)
        {
            _cache.Set(UserId, session, _options);



            return new ApiRes(ApiCode.Success, "登录成功", session);
        }


        [HttpPost("timecap/loginwut")]
        public ApiRes LoginWut([Required] string UserId, [Required] string session)
        {
            _cache.Set(UserId, session, _options);
            Response.Redirect(
                "http://ias.sso.itoken.team/portal.php?posturl=https%3A%2F%2Flucky-day.itoken.team%2Flucky_2019%2flogin%2fias&continueurl=");



            return new ApiRes(ApiCode.Success, "登录成功", session);
        }


        /// <summary>
        /// 添加一个胶囊
        /// </summary>
        /// <param name="UserId">用户Id</param>
        /// <param name="Story">故事</param>
        /// <param name="Address">地点</param>
        /// <param name="session">鉴权</param>
        /// <returns></returns>
        [HttpPost("timecap/add")]
        public ApiRes AddItem([Required] string UserId, [Required] string Address, [Required] string Story, [Required] string session)
        {
            _context.Caps.Add(new Caps(UserId, Address, Story));
            _context.SaveChanges();
            return new ApiRes(ApiCode.Success, "添加胶囊成功", Story);
        }

        /// <summary>
        /// 删除用户的胶囊
        /// </summary>
        /// <param name="UserId">用户Id</param>
        /// <param name="CapId">胶囊Id</param>
        /// <param name="session">鉴权</param>
        /// <returns></returns>

        [HttpDelete("timecap/remove")]
        public ApiRes Remove([Required] string UserId,[Required] int CapId,[Required] string session)
        {
            var cap = _context.Caps.Find(CapId);
            if (cap == null)
            {
                return new ApiRes(ApiCode.Error, "不存在此胶囊", null);
            }

            if (cap.UserId == UserId)
            {
                _context.Remove(cap);
                _context.SaveChanges();
                return new ApiRes(ApiCode.Success, "胶囊删除成功", null);
            }

            return new ApiRes(ApiCode.Error, "用户错误", null);
        }

        /// <summary>
        /// 查询用户拥有的胶囊
        /// </summary>
        /// <param name="UserId">用户Id</param>
        /// <param name="session">鉴权</param>
        /// <returns></returns>
        [HttpPost("timecap/query/own")]
        public ApiRes CapsQueryOwn([Required] string UserId, [Required] string session)
        {
            var caps = (from item in _context.Caps
                where item.UserId == UserId
                select item).AsNoTracking();
            return new ApiRes(ApiCode.Success, "查询成功", caps);
        }

        /// <summary>
        /// 查询用户挖到的胶囊
        /// </summary>
        /// <param name="UserId">用户Id</param>
        /// <param name="session">鉴权</param>
        /// <returns></returns>
        [HttpPost("timecap/query/dig")]
        public ApiRes CapsQueryDig([Required] string UserId, [Required] string session)
        {
            var caps = (from c in _context.Caps
                       where (from item in _context.CapDigs
                              where item.UserDig == UserId
                              select item.CapId).Contains(c.CapId)
                              select c).AsNoTracking();
            return new ApiRes(ApiCode.Success, "查询成功", caps);
        }

        /// <summary>
        /// 挖时光胶囊
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="address">挖掘地点</param>
        /// <param name="session">鉴权</param>
        /// <returns></returns>
        [HttpPost("timecap/dig")]
        public ApiRes Dig([Required] string userId, [Required] string address, [Required] string session)
        {
            var capIds = (from c in _context.Caps 
                         where c.Address == address && !(from item in _context.CapDigs
                                                         where item.UserDig == userId
                                                         select item.CapId).Contains(c.CapId)
                        select c.CapId).ToList();
            var rand = new Random();
            var capId = capIds[rand.Next(capIds.Count)];
            var cap = _context.Caps.Find(capId);
            _context.CapDigs.Add(new CapDig(userId, capId));
            _context.SaveChanges();
            return new ApiRes(ApiCode.Success, "成功挖到了", cap);
        }
    }
}