﻿using System;
using System.Collections.Generic;
using System.Linq;

using EnsureThat;

using Ralfred.Common.DataAccess.Entities;
using Ralfred.Common.DataAccess.Repositories.Abstractions;


namespace Ralfred.Common.DataAccess.Repositories.InMemory
{
	public class InMemoryGroupRepository : IGroupRepository
	{
		public InMemoryGroupRepository()
		{
			_storage = new List<Group>();
		}

		public bool Exists(string name, string path)
		{
			EnsureArg.IsNotEmptyOrWhiteSpace(name);

			return _storage.Any(x =>
				x.Name.Equals(name, StringComparison.OrdinalIgnoreCase) && x.Path.Equals(path, StringComparison.OrdinalIgnoreCase));
		}

		public Group? Get(string name, string path)
		{
			EnsureArg.IsNotEmptyOrWhiteSpace(name);

			return _storage.SingleOrDefault(x =>
				x.Name.Equals(name, StringComparison.OrdinalIgnoreCase) && x.Path.Equals(path, StringComparison.OrdinalIgnoreCase));
		}

		public Guid CreateGroup(string name, string path)
		{
			EnsureArg.IsNotEmptyOrWhiteSpace(name);

			var group = new Group
			{
				Id = Guid.NewGuid(),
				Name = name,
				Path = path
			};

			_storage.Add(group);

			return group.Id;
		}

		public void DeleteGroup(string name, string path)
		{
			EnsureArg.IsNotEmptyOrWhiteSpace(name);

			var items = _storage.Where(x =>
					x.Name.Equals(name, StringComparison.OrdinalIgnoreCase) && x.Path.Equals(path, StringComparison.OrdinalIgnoreCase))
				.ToList();

			items.ForEach(x => _storage.Remove(x));
		}

		private readonly List<Group> _storage;
	}
}