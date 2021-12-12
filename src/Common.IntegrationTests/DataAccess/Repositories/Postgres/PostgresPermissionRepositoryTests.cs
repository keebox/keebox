﻿using System;
using System.Linq;

using AutoFixture;

using FluentAssertions;

using Keebox.Common.DataAccess.Entities;
using Keebox.Common.DataAccess.Repositories.Postgres;
using Keebox.Common.DataAccess.Repositories.Postgres.Transactions;
using Keebox.Common.Types;

using NUnit.Framework;


namespace Keebox.Common.IntegrationTests.DataAccess.Repositories.Postgres
{
	[TestFixture]
	[Category("Integration")]
	public class PostgresPermissionRepositoryTests
	{
		[SetUp]
		public void Setup()
		{
			var storageConnection = new StorageConnection
			{
				ConnectionString = ConfigurationHelper.PostgresDatabaseConnectionString
			};

			_transactionScopeFactory = new TransactionScopeFactory();
			_target = new PostgresPermissionRepository(new ConnectionFactory(storageConnection));

			_transaction = _transactionScopeFactory.BeginTransaction();
		}

		[TearDown]
		public void TearDown()
		{
			_transaction.Dispose();
		}

		[Test]
		public void ExistsTest()
		{
			// arrange
			var groupId = _fixture.Create<Guid>();
			var roleId = _fixture.Create<Guid>();

			var permissionId = _target.Create(new Permission
			{
				Id = Guid.Empty,
				GroupId = groupId,
				RoleId = roleId,
				IsReadOnly = false
			});

			// act
			var result = _target.Exists(permissionId);

			// assert
			result.Should().BeTrue();
		}

		[Test]
		public void GetByGroupIdTest()
		{
			// arrange
			var groupId = _fixture.Create<Guid>();
			var roleId = _fixture.Create<Guid>();

			var permissionId = _target.Create(new Permission
			{
				Id = Guid.Empty,
				GroupId = groupId,
				RoleId = roleId,
				IsReadOnly = false
			});

			// act
			var result = _target.GetByGroupId(groupId).ToArray();

			// assert
			result.Should().HaveCount(1);
			result.Single().Id.Should().Be(permissionId);
		}

		[Test]
		public void GetTest()
		{
			// arrange
			var groupId = _fixture.Create<Guid>();
			var roleId = _fixture.Create<Guid>();

			var permission = new Permission
			{
				Id = Guid.Empty,
				GroupId = groupId,
				RoleId = roleId,
				IsReadOnly = false
			};

			var permissionId = _target.Create(permission);

			// act
			var result = _target.Get(permissionId);

			// assert
			result.Should().BeEquivalentTo(permission, e => e.Excluding(x => x.Id));
		}

		[Test]
		public void DeleteTest()
		{
			// arrange
			var groupId = _fixture.Create<Guid>();
			var roleId = _fixture.Create<Guid>();

			var permission = new Permission
			{
				Id = Guid.Empty,
				GroupId = groupId,
				RoleId = roleId,
				IsReadOnly = false
			};

			var permissionId = _target.Create(permission);

			// act
			_target.Delete(permissionId);

			// assert
			_target.Exists(permissionId).Should().BeFalse();
		}

		[Test]
		public void UpdateTest()
		{
			// arrange
			var groupId = _fixture.Create<Guid>();
			var roleId = _fixture.Create<Guid>();

			var permission = new Permission
			{
				Id = Guid.Empty,
				GroupId = groupId,
				RoleId = roleId,
				IsReadOnly = false
			};

			var permissionId = _target.Create(permission);

			var newPermission = new Permission
			{
				Id = permissionId,
				GroupId = groupId,
				RoleId = roleId,
				IsReadOnly = true
			};

			// act
			_target.Update(new Permission
			{
				Id = permissionId,
				GroupId = groupId,
				RoleId = roleId,
				IsReadOnly = true
			});

			// assert
			_target.Get(permissionId).Should().BeEquivalentTo(newPermission);
		}

		private readonly Fixture _fixture = new();

		private ITransactionScope _transaction;
		private ITransactionScopeFactory _transactionScopeFactory;

		private PostgresPermissionRepository _target;
	}
}