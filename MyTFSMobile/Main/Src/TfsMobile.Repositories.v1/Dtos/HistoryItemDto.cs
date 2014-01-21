using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using TfsMobile.Contracts;

namespace TfsMobile.Repositories.v1.Dtos
{
    public class HistoryItemDto
    {
        public int Id { get; set; }

        public string WorkType { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime HistoryDate { get; set; }

        public Uri TfsItemUri { get; set; }

        public string AreaPath { get; set; }

        public string IterationPath { get; set; }

        public string State { get; set; }

    }

    internal static class AutoMapperBootstrapper
    {
        internal static void Configure()
        {
            Mapper.Initialize(cfg => cfg.AddProfile(new HistoryProfile()));
        }
    }

    internal class HistoryProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<HistoryItemContract, HistoryItemDto>();
            Mapper.AssertConfigurationIsValid();
        }
    }
}
