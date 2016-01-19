﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Mvvm;
using Parse;
using PJA_Skills_032.Model;
using PJA_Skills_032.ParseObjects;

namespace PJA_Skills_032.ViewModel
{
    public class HomeViewModel : BindableBase
    {
        // atom intrusion
        private ObservableCollection<TestUser> _usersObservableCollection = new ObservableCollection<TestUser>();
        public string Width { get; set; } = "777";

        public ObservableCollection<TestUser> UsersObservableCollection
        {
            get { return this._usersObservableCollection; }
            set { this.SetProperty(ref this._usersObservableCollection, value); }
        }

        public HomeViewModel()
        {
            //TestUser testUser = new TestUser("Tim Cook", "Informatyka");
            //TestUser testUser2 = new TestUser("Richard Brenson", "SNM");

            //Users.Add(testUser);
            //Users.Add(testUser2);
        }

        #region methods

        /// <summary>
        /// download ParseObject of all users
        /// </summary>
        /// <returns></returns>
        private async Task<ObservableCollection<TestUser>> GetAllUsers()
        {
            ParseQuery<ParseUser> query = from item in ParseUser.Query
                                          orderby item.CreatedAt
                                          select item;

            IEnumerable<TestUser> allItems = from item in await query.FindAsync()
                                             select new TestUser(item);

            var itemList = new ObservableCollection<TestUser>(allItems);
            return itemList;
        }


        /// <summary>
        /// Add users to list after downloading
        /// </summary>
        /// <returns></returns>
        public async Task AddDownloadedUsers()
        {
            var downloadedUsers = await GetAllUsers();
            foreach (TestUser user in downloadedUsers)
            {
                //Don't show current user in "Home"
                if (ParseUser.CurrentUser.ObjectId != user.BackingObject.ObjectId)
                {
                    await user.GetSkills(); // get skills relationship items
                    UsersObservableCollection.Add(user);
                }
            }
        }


        //public async Task<ObservableCollection<TestUser>> GetAllUsers()
        //{
        //    var allItems = await GetUsersParseObject();
        //    var itemList = new ObservableCollection<TestUser>(allItems);

        //    return itemList;
        //}



        #endregion

    }
}
