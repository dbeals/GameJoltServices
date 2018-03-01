#region File Header
/***********************************************************************
 * Copyright © 2013 Beals Software
 * All Rights Reserved
************************************************************************
Author: Donald Beals
Date: February 4th, 2013
Description: TODO: Write a description of this file here.
****************************** Change Log ******************************
02.04.13 - Created initial file. (dbeals)
***********************************************************************/
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#endregion

namespace GameJolt
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class LoggedInUser : User
	{
		#region Variables
		#endregion

		#region Properties
		public UserIndex UserIndex
		{
			get;
			internal set;
		}

		public string UserToken
		{
			get;
			private set;
		}
		#endregion

		#region Constructors
		public LoggedInUser(User baseInformation, string userToken, UserIndex userIndex)
		{
			this.ID = baseInformation.ID;
			this.Type = baseInformation.Type;
			this.Username = baseInformation.Username;
			this.AvatarUrl = baseInformation.AvatarUrl;
			this.SignedUp = baseInformation.SignedUp;
			this.LastLoggedIn = baseInformation.LastLoggedIn;
			this.Status = baseInformation.Status;
			this.DeveloperName = baseInformation.DeveloperName;
			this.DeveloperWebsite = baseInformation.DeveloperWebsite;
			this.DeveloperDescription = baseInformation.DeveloperDescription;
			this.UserToken = userToken;
			this.UserIndex = userIndex;
		}
		#endregion

		#region Methods
		#endregion
	}
}
