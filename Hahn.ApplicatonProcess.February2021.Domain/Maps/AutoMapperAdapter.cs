using AutoMapper;
using AutoMapper.QueryableExtensions;
using System.Collections.Generic;
using System.Linq;

namespace Hahn.ApplicatonProcess.February2021.Domain.Maps
{
    public class AutoMapperAdapter : IAutoMapper
    {
        private readonly IMapper _mapper;

        public AutoMapperAdapter(IMapper mapper)
        {
            _mapper = mapper;
        }

        public IConfigurationProvider Configuration => _mapper.ConfigurationProvider;

        public T Map<T>(object objectToMap)
        {
            return _mapper.Map<T>(objectToMap);
        }

        public TResult[] Map<TSource, TResult>(IEnumerable<TSource> sourceQuery)
        {
            return sourceQuery.Select(x => _mapper.Map<TResult>(x)).ToArray();
        }

        public IQueryable<TResult> Map<TSource, TResult>(IQueryable<TSource> sourceQuery)
        {
            return sourceQuery.ProjectTo<TResult>(_mapper.ConfigurationProvider);
        }

        public void Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            _mapper.Map(source, destination);
        }

        public TDestination Map<TSource, TDestination>(TSource groupCodeModel)
        {
            return _mapper.Map<TSource, TDestination>(groupCodeModel);
        }
    }
}
