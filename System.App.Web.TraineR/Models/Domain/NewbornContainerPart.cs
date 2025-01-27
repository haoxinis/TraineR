﻿using System;
using System.App.Model.Enums;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.App.Model.Resources;

namespace System.App.Web.TraineR.Models.Domain
{
    public partial class NewbornContainer
    {
        /// <summary>
        /// Dyanmically use specific language version database.
        /// </summary>
        /// <remarks>
        /// Thread.CurrentThread.CurrentUICulture does not work. We use Resource.lan
        /// </remarks>
        public NewbornContainer(string lan):base("name=quizbank." + lan)
        {
            Debug.WriteLine(lan);
        }

        public NewbornContainer() : base("name=quizbank.en")
        {
            // Debug.WriteLine(Resource.lan.ToLower());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            //modelBuilder.Conventions.Remove<PluralizingEntitySetNameConvention>();
            //modelBuilder.Entity<Newborn>().ToTable("Newborn"); // default table name is xxxSet
            base.OnModelCreating(modelBuilder);
        }

        

        /// <summary>
        /// Only return images that are not deleted
        /// </summary>
        public IQueryable<Image> ImageQuery
        {
            get
            {
                return this.Image.Where(x => x.Deleted != true);
            }
        }

        public IQueryable<Image> FundusImageQuery
        {
            get
            {
                return this.ImageQuery.Where(x => x.Type == "眼底彩照" || x.Type == "fundus" || x.Type != null && x.Type.Contains("RetCam"));
            }
        }


        public IQueryable<Image> FundusImageForQuizQuery
        {
            get
            {
                return this.FundusImageQuery.Where(x => x.Tag06 == true);
            }
        }

        public IQueryable<Dict_Diagnosis> DiagnosisStandardTermQuery
        {
            get
            {
                return this.Dict_Diagnosis.Where(x => x.CodingSystem == "ROP_MST");
            }
        }

        public IQueryable<Dict_Diagnosis> DiagnosisPrivateTermQuery
        {
            get
            {
                return this.Dict_Diagnosis.Where(x => x.CodingSystem != "ROP_MST");
            }
        }

        public void DeleteImage(int id)
        {
            var instance = this.Image.Find(id);
            if (instance != null)
            {
                instance.Deleted = true;
                this.Entry(instance).State = System.Data.Entity.EntityState.Modified;
                this.SaveChanges();
            }
        }

    }
}