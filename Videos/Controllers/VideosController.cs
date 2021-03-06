﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Videos.Models;

namespace Videos.Controllers
{
    public class VideosController : ApiController
    {
        VideoDb _db;
        public VideosController()
        {
            _db = new VideoDb();
            _db.Configuration.ProxyCreationEnabled = false;
        }
        //GET api/videos
        public IEnumerable<Video> GetAllVideos()
        {
            return _db.Videos;
        }
        //GET api/videos/5
        public Video Get(int id)
        {
            var video = _db.Videos.Find(id);
            if (video == default(Video))
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }
            return video;
        }
        //POST api/videos
        public HttpResponseMessage PostVideo(Video video)
        {
            if (ModelState.IsValid)
            {
                _db.Videos.Add(video);
                _db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, video);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = video.Id }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }
        //PUT api/videos/5
        public HttpResponseMessage PutVideo(int id, Video video)
        {
            if (ModelState.IsValid && id == video.Id)
            {
                _db.Entry<Video>(video).State = EntityState.Modified;
                try
                {
                    _db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }
                return Request.CreateResponse(HttpStatusCode.OK, video);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

        }
        //DELETE api/videos/5
        public HttpResponseMessage DeleteVideo(int id)
        {
            var video = _db.Videos.Find(id);
            if (video == default(Video))
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            else
            {
                _db.Videos.Remove(video);
            }
            try
            {
                _db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, video);
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }
    }
}
