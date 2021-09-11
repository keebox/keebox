﻿using System.Collections.Generic;

using Ralfred.Common.DataAccess.Context;
using Ralfred.Common.DataAccess.Entities;
using Ralfred.Common.Helpers;


namespace Ralfred.Common.DataAccess.Repositories
{
	public class GroupRepository : IGroupRepository
	{
		public GroupRepository(IStorageContext<Group> groupContext, IStorageContext<Secret> secretContext)
		{
			_groupContext = groupContext;
			_secretContext = secretContext;
		}

		#region Implementation of IGroupRepository

		// TODO: circular dependency
		public bool Exists(string name, string path)
		{
			var group = _groupContext.Find(g => g.Path.Equals(path) && g.Name.Equals(name));

			return group != null;
		}

		public Group? FindByPath(string path, string name)
		{
			var group = _groupContext.Find(g => g.Path.Equals(path) && g.Name.Equals(name));

			return group;
		}

		public void CreateGroup(string name, string path, Dictionary<string, string> secrets, Dictionary<string, string> files)
		{
			var group = _groupContext.Add(new Group
			{
				Name = name,
				Path = path
			});

			foreach (var (key, value) in secrets)
			{
				_secretContext.Add(new Secret
				{
					Name = key,
					Value = value,
					GroupId = group.Id,
					IsFile = false
				});
			}

			foreach (var (key, value) in files)
			{
				_secretContext.Add(new Secret
				{
					Name = key,
					Value = value,
					GroupId = group.Id,
					IsFile = true
				});
			}

		}

		#endregion

		private readonly IStorageContext<Group> _groupContext;
		private readonly IStorageContext<Secret> _secretContext;
	}
}