using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Catalog.Models;

namespace Catalog.Repo
{
	public interface IMetaCategoriesRepo
	{
		Task<ResponseBuilder> All();

		Task<ResponseBuilder> Register(MetaCategory metaCategory);

		Task<ResponseBuilder> Unregiester(int ID);

		Task<ResponseBuilder> ById(int ID);
	}

	public sealed class FakeMetaCategoriesRepo : IMetaCategoriesRepo
	{
		private static readonly IDictionary<int, MetaCategory> _DB =
			new Dictionary<int, MetaCategory>
				{
					{
						0,
						new MetaCategory
							{
								ID = 0,
								Description = "Description for metacategory 1.",
								Title = "MetaCategory 1"
							}
					}
				};

		public Task<ResponseBuilder> All()
		{
			return Task.FromResult(_DB.Values.Success());
		}

		public Task<ResponseBuilder> Register(MetaCategory metaCategory)
		{
			_DB.Add(_DB.Count, metaCategory);
			return Task.FromResult($"'{metaCategory.Title}' registered successfully.".Success());
		}

		public Task<ResponseBuilder> Unregiester(int ID)
		{
			if (_DB.ContainsKey(ID))
			{
				var toBeRemovedItem = _DB[ID];
				_DB.Remove(ID);
				return Task.FromResult($"'{toBeRemovedItem.Title}' removed successfully.".Success());
			}

			return Task.FromResult($"MetaCategory with ID={ID} does not exist.".Failure());
		}

		public Task<ResponseBuilder> ById(int ID)
		{
			return TryCatchHelper.TryCatchAsync(
				() => Task.FromResult(_DB[ID].Success()),
				new Dictionary<Type, Func<Exception, string>>
					{
						{
							typeof(KeyNotFoundException),
							exception => $"MetaCategory with ID={ID} does not exist."
						}
					});
		}
	}
}