﻿using HelloWorldWebApp.Model;
using Marten;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace HelloWorldWebApp.Controllers
{
    [Route("/posts")]
    public class BlogPostController
    {
        private readonly IDocumentStore _documentStore;

        public BlogPostController(IDocumentStore documentStore)
        {
            _documentStore = documentStore;
        }

        [HttpGet]
        public IEnumerable<BlogPost> Get()
        {
            using (var session = _documentStore.QuerySession())
            {
                return session.Query<BlogPost>();
            }
        }

        [HttpGet("{id}")]
        public BlogPost Get(int id)
        {
            using (var session = _documentStore.QuerySession())
            {
                return session
                    .Query<BlogPost>()
                    .Where(post => post.Id == id)
                    .FirstOrDefault();

            }
        }

        [HttpPost]
        public BlogPost Create([FromBody]BlogPost post)
        {
            using (var session = _documentStore.LightweightSession())
            {
                session.Store(post);
                session.SaveChanges();
                return post;
            }
        }
    }
}
