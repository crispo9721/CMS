﻿using CMS.Context;
using CMS.Models.Db.Article;
using CMS.Services.interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Services
{
    public class TagService : ITagService
    {
		private readonly CMSContext _context;

		public TagService(CMSContext context)
		{
			_context = context;
		}

		public async Task<bool> Create(TagModel tag)
		{
			await _context.Tags.AddAsync(tag);
			return await _context.SaveChangesAsync() > 0;
		}

		public async Task<TagModel> Get(int id)
		{
			return await _context.Tags.SingleOrDefaultAsync(b => b.Id == id);
		}

		public async Task<List<TagModel>> GetAll()
		{
			return await _context.Tags.ToListAsync();
		}

		public async Task<bool> Update(TagModel tag)
		{
			_context.Tags.Update(tag);
			return await _context.SaveChangesAsync() > 0;
		}

		public async Task<bool> Delete(int id)
		{
			var tag = await _context.Tags.SingleOrDefaultAsync(b => b.Id == id);

			if (tag == null)
			{
				return false;
			}

			_context.Tags.Remove(tag);
			return await _context.SaveChangesAsync() > 0;
		}
	}
}