﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18010
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HockeyTracker.Data
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Runtime.Serialization;
	using System.ComponentModel;
	using System;
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="db658")]
	public partial class DataLayerDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    partial void InsertGame(Game instance);
    partial void UpdateGame(Game instance);
    partial void DeleteGame(Game instance);
    partial void InsertNotificationsHistory(NotificationsHistory instance);
    partial void UpdateNotificationsHistory(NotificationsHistory instance);
    partial void DeleteNotificationsHistory(NotificationsHistory instance);
    partial void InsertSubscription(Subscription instance);
    partial void UpdateSubscription(Subscription instance);
    partial void DeleteSubscription(Subscription instance);
    partial void InsertUser(User instance);
    partial void UpdateUser(User instance);
    partial void DeleteUser(User instance);
    #endregion
		
		public DataLayerDataContext() : 
				base(global::HockeyTracker.Data.Properties.Settings.Default.HockeyTrackerConnectionString_Prod, mappingSource)
		{
			OnCreated();
		}
		
		public DataLayerDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public DataLayerDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public DataLayerDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public DataLayerDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<Game> Games
		{
			get
			{
				return this.GetTable<Game>();
			}
		}
		
		public System.Data.Linq.Table<NotificationsHistory> NotificationsHistories
		{
			get
			{
				return this.GetTable<NotificationsHistory>();
			}
		}
		
		public System.Data.Linq.Table<Subscription> Subscriptions
		{
			get
			{
				return this.GetTable<Subscription>();
			}
		}
		
		public System.Data.Linq.Table<User> Users
		{
			get
			{
				return this.GetTable<User>();
			}
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.GetInstallationCount")]
		public ISingleResult<GetInstallationCountResult> GetInstallationCount([global::System.Data.Linq.Mapping.ParameterAttribute(Name="ForDate", DbType="VarChar(10)")] string forDate)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), forDate);
			return ((ISingleResult<GetInstallationCountResult>)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.GetUsersForNotification")]
		public ISingleResult<GetUsersForNotificationResult> GetUsersForNotification([global::System.Data.Linq.Mapping.ParameterAttribute(Name="Team", DbType="VarChar(5)")] string team, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="SubscriptionType", DbType="VarChar(20)")] string subscriptionType)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), team, subscriptionType);
			return ((ISingleResult<GetUsersForNotificationResult>)(result.ReturnValue));
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.Games")]
	[global::System.Runtime.Serialization.DataContractAttribute()]
	public partial class Game : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _GameNumber;
		
		private System.DateTime _GameTime;
		
		private string _HomeTeam;
		
		private string _VisitorTeam;
		
		private int _HomeGoals;
		
		private int _VisitorGoals;
		
		private System.Nullable<System.DateTime> _Started;
		
		private System.Nullable<System.DateTime> _Ended;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnGameNumberChanging(int value);
    partial void OnGameNumberChanged();
    partial void OnGameTimeChanging(System.DateTime value);
    partial void OnGameTimeChanged();
    partial void OnHomeTeamChanging(string value);
    partial void OnHomeTeamChanged();
    partial void OnVisitorTeamChanging(string value);
    partial void OnVisitorTeamChanged();
    partial void OnHomeGoalsChanging(int value);
    partial void OnHomeGoalsChanged();
    partial void OnVisitorGoalsChanging(int value);
    partial void OnVisitorGoalsChanged();
    partial void OnStartedChanging(System.Nullable<System.DateTime> value);
    partial void OnStartedChanged();
    partial void OnEndedChanging(System.Nullable<System.DateTime> value);
    partial void OnEndedChanged();
    #endregion
		
		public Game()
		{
			this.Initialize();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_GameNumber", DbType="Int NOT NULL", IsPrimaryKey=true)]
		[global::System.Runtime.Serialization.DataMemberAttribute(Order=1)]
		public int GameNumber
		{
			get
			{
				return this._GameNumber;
			}
			set
			{
				if ((this._GameNumber != value))
				{
					this.OnGameNumberChanging(value);
					this.SendPropertyChanging();
					this._GameNumber = value;
					this.SendPropertyChanged("GameNumber");
					this.OnGameNumberChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_GameTime", DbType="SmallDateTime NOT NULL")]
		[global::System.Runtime.Serialization.DataMemberAttribute(Order=2)]
		public System.DateTime GameTime
		{
			get
			{
				return this._GameTime;
			}
			set
			{
				if ((this._GameTime != value))
				{
					this.OnGameTimeChanging(value);
					this.SendPropertyChanging();
					this._GameTime = value;
					this.SendPropertyChanged("GameTime");
					this.OnGameTimeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_HomeTeam", DbType="NChar(3) NOT NULL", CanBeNull=false)]
		[global::System.Runtime.Serialization.DataMemberAttribute(Order=3)]
		public string HomeTeam
		{
			get
			{
				return this._HomeTeam;
			}
			set
			{
				if ((this._HomeTeam != value))
				{
					this.OnHomeTeamChanging(value);
					this.SendPropertyChanging();
					this._HomeTeam = value;
					this.SendPropertyChanged("HomeTeam");
					this.OnHomeTeamChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_VisitorTeam", DbType="NChar(3) NOT NULL", CanBeNull=false)]
		[global::System.Runtime.Serialization.DataMemberAttribute(Order=4)]
		public string VisitorTeam
		{
			get
			{
				return this._VisitorTeam;
			}
			set
			{
				if ((this._VisitorTeam != value))
				{
					this.OnVisitorTeamChanging(value);
					this.SendPropertyChanging();
					this._VisitorTeam = value;
					this.SendPropertyChanged("VisitorTeam");
					this.OnVisitorTeamChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_HomeGoals", DbType="Int NOT NULL")]
		[global::System.Runtime.Serialization.DataMemberAttribute(Order=5)]
		public int HomeGoals
		{
			get
			{
				return this._HomeGoals;
			}
			set
			{
				if ((this._HomeGoals != value))
				{
					this.OnHomeGoalsChanging(value);
					this.SendPropertyChanging();
					this._HomeGoals = value;
					this.SendPropertyChanged("HomeGoals");
					this.OnHomeGoalsChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_VisitorGoals", DbType="Int NOT NULL")]
		[global::System.Runtime.Serialization.DataMemberAttribute(Order=6)]
		public int VisitorGoals
		{
			get
			{
				return this._VisitorGoals;
			}
			set
			{
				if ((this._VisitorGoals != value))
				{
					this.OnVisitorGoalsChanging(value);
					this.SendPropertyChanging();
					this._VisitorGoals = value;
					this.SendPropertyChanged("VisitorGoals");
					this.OnVisitorGoalsChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Started", DbType="SmallDateTime")]
		[global::System.Runtime.Serialization.DataMemberAttribute(Order=7)]
		public System.Nullable<System.DateTime> Started
		{
			get
			{
				return this._Started;
			}
			set
			{
				if ((this._Started != value))
				{
					this.OnStartedChanging(value);
					this.SendPropertyChanging();
					this._Started = value;
					this.SendPropertyChanged("Started");
					this.OnStartedChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Ended", DbType="SmallDateTime")]
		[global::System.Runtime.Serialization.DataMemberAttribute(Order=8)]
		public System.Nullable<System.DateTime> Ended
		{
			get
			{
				return this._Ended;
			}
			set
			{
				if ((this._Ended != value))
				{
					this.OnEndedChanging(value);
					this.SendPropertyChanging();
					this._Ended = value;
					this.SendPropertyChanged("Ended");
					this.OnEndedChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		
		private void Initialize()
		{
			OnCreated();
		}
		
		[global::System.Runtime.Serialization.OnDeserializingAttribute()]
		[global::System.ComponentModel.EditorBrowsableAttribute(EditorBrowsableState.Never)]
		public void OnDeserializing(StreamingContext context)
		{
			this.Initialize();
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.NotificationsHistory")]
	[global::System.Runtime.Serialization.DataContractAttribute()]
	public partial class NotificationsHistory : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private long _Id;
		
		private long _UserId;
		
		private System.DateTime _Time;
		
		private string _Title;
		
		private string _Message;
		
		private string _StatusCode;
		
		private string _NotificationStatus;
		
		private string _DeviceConnectionStatus;
		
		private string _SubscriptionStatus;
		
		private EntityRef<User> _User;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIdChanging(long value);
    partial void OnIdChanged();
    partial void OnUserIdChanging(long value);
    partial void OnUserIdChanged();
    partial void OnTimeChanging(System.DateTime value);
    partial void OnTimeChanged();
    partial void OnTitleChanging(string value);
    partial void OnTitleChanged();
    partial void OnMessageChanging(string value);
    partial void OnMessageChanged();
    partial void OnStatusCodeChanging(string value);
    partial void OnStatusCodeChanged();
    partial void OnNotificationStatusChanging(string value);
    partial void OnNotificationStatusChanged();
    partial void OnDeviceConnectionStatusChanging(string value);
    partial void OnDeviceConnectionStatusChanged();
    partial void OnSubscriptionStatusChanging(string value);
    partial void OnSubscriptionStatusChanged();
    #endregion
		
		public NotificationsHistory()
		{
			this.Initialize();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Id", AutoSync=AutoSync.OnInsert, DbType="BigInt NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		[global::System.Runtime.Serialization.DataMemberAttribute(Order=1)]
		public long Id
		{
			get
			{
				return this._Id;
			}
			set
			{
				if ((this._Id != value))
				{
					this.OnIdChanging(value);
					this.SendPropertyChanging();
					this._Id = value;
					this.SendPropertyChanged("Id");
					this.OnIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_UserId", DbType="BigInt NOT NULL")]
		[global::System.Runtime.Serialization.DataMemberAttribute(Order=2)]
		public long UserId
		{
			get
			{
				return this._UserId;
			}
			set
			{
				if ((this._UserId != value))
				{
					if (this._User.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.OnUserIdChanging(value);
					this.SendPropertyChanging();
					this._UserId = value;
					this.SendPropertyChanged("UserId");
					this.OnUserIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Time", DbType="DateTime NOT NULL")]
		[global::System.Runtime.Serialization.DataMemberAttribute(Order=3)]
		public System.DateTime Time
		{
			get
			{
				return this._Time;
			}
			set
			{
				if ((this._Time != value))
				{
					this.OnTimeChanging(value);
					this.SendPropertyChanging();
					this._Time = value;
					this.SendPropertyChanged("Time");
					this.OnTimeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Title", DbType="VarChar(50)")]
		[global::System.Runtime.Serialization.DataMemberAttribute(Order=4)]
		public string Title
		{
			get
			{
				return this._Title;
			}
			set
			{
				if ((this._Title != value))
				{
					this.OnTitleChanging(value);
					this.SendPropertyChanging();
					this._Title = value;
					this.SendPropertyChanged("Title");
					this.OnTitleChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Message", DbType="VarChar(255)")]
		[global::System.Runtime.Serialization.DataMemberAttribute(Order=5)]
		public string Message
		{
			get
			{
				return this._Message;
			}
			set
			{
				if ((this._Message != value))
				{
					this.OnMessageChanging(value);
					this.SendPropertyChanging();
					this._Message = value;
					this.SendPropertyChanged("Message");
					this.OnMessageChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_StatusCode", DbType="VarChar(50)")]
		[global::System.Runtime.Serialization.DataMemberAttribute(Order=6)]
		public string StatusCode
		{
			get
			{
				return this._StatusCode;
			}
			set
			{
				if ((this._StatusCode != value))
				{
					this.OnStatusCodeChanging(value);
					this.SendPropertyChanging();
					this._StatusCode = value;
					this.SendPropertyChanged("StatusCode");
					this.OnStatusCodeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_NotificationStatus", DbType="VarChar(50)")]
		[global::System.Runtime.Serialization.DataMemberAttribute(Order=7)]
		public string NotificationStatus
		{
			get
			{
				return this._NotificationStatus;
			}
			set
			{
				if ((this._NotificationStatus != value))
				{
					this.OnNotificationStatusChanging(value);
					this.SendPropertyChanging();
					this._NotificationStatus = value;
					this.SendPropertyChanged("NotificationStatus");
					this.OnNotificationStatusChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DeviceConnectionStatus", DbType="VarChar(50)")]
		[global::System.Runtime.Serialization.DataMemberAttribute(Order=8)]
		public string DeviceConnectionStatus
		{
			get
			{
				return this._DeviceConnectionStatus;
			}
			set
			{
				if ((this._DeviceConnectionStatus != value))
				{
					this.OnDeviceConnectionStatusChanging(value);
					this.SendPropertyChanging();
					this._DeviceConnectionStatus = value;
					this.SendPropertyChanged("DeviceConnectionStatus");
					this.OnDeviceConnectionStatusChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_SubscriptionStatus", DbType="VarChar(50)")]
		[global::System.Runtime.Serialization.DataMemberAttribute(Order=9)]
		public string SubscriptionStatus
		{
			get
			{
				return this._SubscriptionStatus;
			}
			set
			{
				if ((this._SubscriptionStatus != value))
				{
					this.OnSubscriptionStatusChanging(value);
					this.SendPropertyChanging();
					this._SubscriptionStatus = value;
					this.SendPropertyChanged("SubscriptionStatus");
					this.OnSubscriptionStatusChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="User_NotificationsHistory", Storage="_User", ThisKey="UserId", OtherKey="Id", IsForeignKey=true)]
		public User User
		{
			get
			{
				return this._User.Entity;
			}
			set
			{
				User previousValue = this._User.Entity;
				if (((previousValue != value) 
							|| (this._User.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._User.Entity = null;
						previousValue.NotificationsHistories.Remove(this);
					}
					this._User.Entity = value;
					if ((value != null))
					{
						value.NotificationsHistories.Add(this);
						this._UserId = value.Id;
					}
					else
					{
						this._UserId = default(long);
					}
					this.SendPropertyChanged("User");
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		
		private void Initialize()
		{
			this._User = default(EntityRef<User>);
			OnCreated();
		}
		
		[global::System.Runtime.Serialization.OnDeserializingAttribute()]
		[global::System.ComponentModel.EditorBrowsableAttribute(EditorBrowsableState.Never)]
		public void OnDeserializing(StreamingContext context)
		{
			this.Initialize();
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.Subscriptions")]
	[global::System.Runtime.Serialization.DataContractAttribute()]
	public partial class Subscription : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private long _Id;
		
		private long _UserId;
		
		private string _Team;
		
		private string _SubscriptionType;
		
		private EntityRef<User> _User;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIdChanging(long value);
    partial void OnIdChanged();
    partial void OnUserIdChanging(long value);
    partial void OnUserIdChanged();
    partial void OnTeamChanging(string value);
    partial void OnTeamChanged();
    partial void OnSubscriptionTypeChanging(string value);
    partial void OnSubscriptionTypeChanged();
    #endregion
		
		public Subscription()
		{
			this.Initialize();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Id", AutoSync=AutoSync.OnInsert, DbType="BigInt NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		[global::System.Runtime.Serialization.DataMemberAttribute(Order=1)]
		public long Id
		{
			get
			{
				return this._Id;
			}
			set
			{
				if ((this._Id != value))
				{
					this.OnIdChanging(value);
					this.SendPropertyChanging();
					this._Id = value;
					this.SendPropertyChanged("Id");
					this.OnIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_UserId", DbType="BigInt NOT NULL")]
		[global::System.Runtime.Serialization.DataMemberAttribute(Order=2)]
		public long UserId
		{
			get
			{
				return this._UserId;
			}
			set
			{
				if ((this._UserId != value))
				{
					if (this._User.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.OnUserIdChanging(value);
					this.SendPropertyChanging();
					this._UserId = value;
					this.SendPropertyChanged("UserId");
					this.OnUserIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Team", DbType="NChar(3) NOT NULL", CanBeNull=false)]
		[global::System.Runtime.Serialization.DataMemberAttribute(Order=3)]
		public string Team
		{
			get
			{
				return this._Team;
			}
			set
			{
				if ((this._Team != value))
				{
					this.OnTeamChanging(value);
					this.SendPropertyChanging();
					this._Team = value;
					this.SendPropertyChanged("Team");
					this.OnTeamChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_SubscriptionType", DbType="VarChar(15) NOT NULL", CanBeNull=false)]
		[global::System.Runtime.Serialization.DataMemberAttribute(Order=4)]
		public string SubscriptionType
		{
			get
			{
				return this._SubscriptionType;
			}
			set
			{
				if ((this._SubscriptionType != value))
				{
					this.OnSubscriptionTypeChanging(value);
					this.SendPropertyChanging();
					this._SubscriptionType = value;
					this.SendPropertyChanged("SubscriptionType");
					this.OnSubscriptionTypeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="User_Subscription", Storage="_User", ThisKey="UserId", OtherKey="Id", IsForeignKey=true)]
		public User User
		{
			get
			{
				return this._User.Entity;
			}
			set
			{
				User previousValue = this._User.Entity;
				if (((previousValue != value) 
							|| (this._User.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._User.Entity = null;
						previousValue.Subscriptions.Remove(this);
					}
					this._User.Entity = value;
					if ((value != null))
					{
						value.Subscriptions.Add(this);
						this._UserId = value.Id;
					}
					else
					{
						this._UserId = default(long);
					}
					this.SendPropertyChanged("User");
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		
		private void Initialize()
		{
			this._User = default(EntityRef<User>);
			OnCreated();
		}
		
		[global::System.Runtime.Serialization.OnDeserializingAttribute()]
		[global::System.ComponentModel.EditorBrowsableAttribute(EditorBrowsableState.Never)]
		public void OnDeserializing(StreamingContext context)
		{
			this.Initialize();
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.Users")]
	[global::System.Runtime.Serialization.DataContractAttribute()]
	public partial class User : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private long _Id;
		
		private System.Guid _AnId;
		
		private string _ChannelUri;
		
		private int _LocalUtcOffset;
		
		private System.DateTime _LastUpdated;
		
		private bool _Active;
		
		private System.Nullable<System.DateTime> _FirstUpdated;
		
		private string _Culture;
		
		private EntitySet<NotificationsHistory> _NotificationsHistories;
		
		private EntitySet<Subscription> _Subscriptions;
		
		private bool serializing;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIdChanging(long value);
    partial void OnIdChanged();
    partial void OnAnIdChanging(System.Guid value);
    partial void OnAnIdChanged();
    partial void OnChannelUriChanging(string value);
    partial void OnChannelUriChanged();
    partial void OnLocalUtcOffsetChanging(int value);
    partial void OnLocalUtcOffsetChanged();
    partial void OnLastUpdatedChanging(System.DateTime value);
    partial void OnLastUpdatedChanged();
    partial void OnActiveChanging(bool value);
    partial void OnActiveChanged();
    partial void OnFirstUpdatedChanging(System.Nullable<System.DateTime> value);
    partial void OnFirstUpdatedChanged();
    partial void OnCultureChanging(string value);
    partial void OnCultureChanged();
    #endregion
		
		public User()
		{
			this.Initialize();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Id", AutoSync=AutoSync.OnInsert, DbType="BigInt NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		[global::System.Runtime.Serialization.DataMemberAttribute(Order=1)]
		public long Id
		{
			get
			{
				return this._Id;
			}
			set
			{
				if ((this._Id != value))
				{
					this.OnIdChanging(value);
					this.SendPropertyChanging();
					this._Id = value;
					this.SendPropertyChanged("Id");
					this.OnIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_AnId", DbType="UniqueIdentifier NOT NULL")]
		[global::System.Runtime.Serialization.DataMemberAttribute(Order=2)]
		public System.Guid AnId
		{
			get
			{
				return this._AnId;
			}
			set
			{
				if ((this._AnId != value))
				{
					this.OnAnIdChanging(value);
					this.SendPropertyChanging();
					this._AnId = value;
					this.SendPropertyChanged("AnId");
					this.OnAnIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ChannelUri", DbType="VarChar(255) NOT NULL", CanBeNull=false)]
		[global::System.Runtime.Serialization.DataMemberAttribute(Order=3)]
		public string ChannelUri
		{
			get
			{
				return this._ChannelUri;
			}
			set
			{
				if ((this._ChannelUri != value))
				{
					this.OnChannelUriChanging(value);
					this.SendPropertyChanging();
					this._ChannelUri = value;
					this.SendPropertyChanged("ChannelUri");
					this.OnChannelUriChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_LocalUtcOffset", DbType="Int NOT NULL")]
		[global::System.Runtime.Serialization.DataMemberAttribute(Order=4)]
		public int LocalUtcOffset
		{
			get
			{
				return this._LocalUtcOffset;
			}
			set
			{
				if ((this._LocalUtcOffset != value))
				{
					this.OnLocalUtcOffsetChanging(value);
					this.SendPropertyChanging();
					this._LocalUtcOffset = value;
					this.SendPropertyChanged("LocalUtcOffset");
					this.OnLocalUtcOffsetChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_LastUpdated", DbType="DateTime NOT NULL")]
		[global::System.Runtime.Serialization.DataMemberAttribute(Order=5)]
		public System.DateTime LastUpdated
		{
			get
			{
				return this._LastUpdated;
			}
			set
			{
				if ((this._LastUpdated != value))
				{
					this.OnLastUpdatedChanging(value);
					this.SendPropertyChanging();
					this._LastUpdated = value;
					this.SendPropertyChanged("LastUpdated");
					this.OnLastUpdatedChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Active", DbType="Bit NOT NULL")]
		[global::System.Runtime.Serialization.DataMemberAttribute(Order=6)]
		public bool Active
		{
			get
			{
				return this._Active;
			}
			set
			{
				if ((this._Active != value))
				{
					this.OnActiveChanging(value);
					this.SendPropertyChanging();
					this._Active = value;
					this.SendPropertyChanged("Active");
					this.OnActiveChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_FirstUpdated", DbType="DateTime")]
		[global::System.Runtime.Serialization.DataMemberAttribute(Order=7)]
		public System.Nullable<System.DateTime> FirstUpdated
		{
			get
			{
				return this._FirstUpdated;
			}
			set
			{
				if ((this._FirstUpdated != value))
				{
					this.OnFirstUpdatedChanging(value);
					this.SendPropertyChanging();
					this._FirstUpdated = value;
					this.SendPropertyChanged("FirstUpdated");
					this.OnFirstUpdatedChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Culture", DbType="NChar(10)")]
		[global::System.Runtime.Serialization.DataMemberAttribute(Order=8)]
		public string Culture
		{
			get
			{
				return this._Culture;
			}
			set
			{
				if ((this._Culture != value))
				{
					this.OnCultureChanging(value);
					this.SendPropertyChanging();
					this._Culture = value;
					this.SendPropertyChanged("Culture");
					this.OnCultureChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="User_NotificationsHistory", Storage="_NotificationsHistories", ThisKey="Id", OtherKey="UserId")]
		[global::System.Runtime.Serialization.DataMemberAttribute(Order=9, EmitDefaultValue=false)]
		public EntitySet<NotificationsHistory> NotificationsHistories
		{
			get
			{
				if ((this.serializing 
							&& (this._NotificationsHistories.HasLoadedOrAssignedValues == false)))
				{
					return null;
				}
				return this._NotificationsHistories;
			}
			set
			{
				this._NotificationsHistories.Assign(value);
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="User_Subscription", Storage="_Subscriptions", ThisKey="Id", OtherKey="UserId")]
		[global::System.Runtime.Serialization.DataMemberAttribute(Order=10, EmitDefaultValue=false)]
		public EntitySet<Subscription> Subscriptions
		{
			get
			{
				if ((this.serializing 
							&& (this._Subscriptions.HasLoadedOrAssignedValues == false)))
				{
					return null;
				}
				return this._Subscriptions;
			}
			set
			{
				this._Subscriptions.Assign(value);
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		
		private void attach_NotificationsHistories(NotificationsHistory entity)
		{
			this.SendPropertyChanging();
			entity.User = this;
		}
		
		private void detach_NotificationsHistories(NotificationsHistory entity)
		{
			this.SendPropertyChanging();
			entity.User = null;
		}
		
		private void attach_Subscriptions(Subscription entity)
		{
			this.SendPropertyChanging();
			entity.User = this;
		}
		
		private void detach_Subscriptions(Subscription entity)
		{
			this.SendPropertyChanging();
			entity.User = null;
		}
		
		private void Initialize()
		{
			this._NotificationsHistories = new EntitySet<NotificationsHistory>(new Action<NotificationsHistory>(this.attach_NotificationsHistories), new Action<NotificationsHistory>(this.detach_NotificationsHistories));
			this._Subscriptions = new EntitySet<Subscription>(new Action<Subscription>(this.attach_Subscriptions), new Action<Subscription>(this.detach_Subscriptions));
			OnCreated();
		}
		
		[global::System.Runtime.Serialization.OnDeserializingAttribute()]
		[global::System.ComponentModel.EditorBrowsableAttribute(EditorBrowsableState.Never)]
		public void OnDeserializing(StreamingContext context)
		{
			this.Initialize();
		}
		
		[global::System.Runtime.Serialization.OnSerializingAttribute()]
		[global::System.ComponentModel.EditorBrowsableAttribute(EditorBrowsableState.Never)]
		public void OnSerializing(StreamingContext context)
		{
			this.serializing = true;
		}
		
		[global::System.Runtime.Serialization.OnSerializedAttribute()]
		[global::System.ComponentModel.EditorBrowsableAttribute(EditorBrowsableState.Never)]
		public void OnSerialized(StreamingContext context)
		{
			this.serializing = false;
		}
	}
	
	[global::System.Runtime.Serialization.DataContractAttribute()]
	public partial class GetInstallationCountResult
	{
		
		private System.Nullable<int> _Count;
		
		public GetInstallationCountResult()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Count", DbType="Int")]
		[global::System.Runtime.Serialization.DataMemberAttribute(Order=1)]
		public System.Nullable<int> Count
		{
			get
			{
				return this._Count;
			}
			set
			{
				if ((this._Count != value))
				{
					this._Count = value;
				}
			}
		}
	}
	
	[global::System.Runtime.Serialization.DataContractAttribute()]
	public partial class GetUsersForNotificationResult
	{
		
		private long _Id;
		
		private System.Guid _AnId;
		
		private string _ChannelUri;
		
		private int _LocalUtcOffset;
		
		private System.DateTime _LastUpdated;
		
		private bool _Active;
		
		private System.Nullable<System.DateTime> _FirstUpdated;
		
		private string _Culture;
		
		public GetUsersForNotificationResult()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Id", DbType="BigInt NOT NULL")]
		[global::System.Runtime.Serialization.DataMemberAttribute(Order=1)]
		public long Id
		{
			get
			{
				return this._Id;
			}
			set
			{
				if ((this._Id != value))
				{
					this._Id = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_AnId", DbType="UniqueIdentifier NOT NULL")]
		[global::System.Runtime.Serialization.DataMemberAttribute(Order=2)]
		public System.Guid AnId
		{
			get
			{
				return this._AnId;
			}
			set
			{
				if ((this._AnId != value))
				{
					this._AnId = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ChannelUri", DbType="VarChar(255) NOT NULL", CanBeNull=false)]
		[global::System.Runtime.Serialization.DataMemberAttribute(Order=3)]
		public string ChannelUri
		{
			get
			{
				return this._ChannelUri;
			}
			set
			{
				if ((this._ChannelUri != value))
				{
					this._ChannelUri = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_LocalUtcOffset", DbType="Int NOT NULL")]
		[global::System.Runtime.Serialization.DataMemberAttribute(Order=4)]
		public int LocalUtcOffset
		{
			get
			{
				return this._LocalUtcOffset;
			}
			set
			{
				if ((this._LocalUtcOffset != value))
				{
					this._LocalUtcOffset = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_LastUpdated", DbType="DateTime NOT NULL")]
		[global::System.Runtime.Serialization.DataMemberAttribute(Order=5)]
		public System.DateTime LastUpdated
		{
			get
			{
				return this._LastUpdated;
			}
			set
			{
				if ((this._LastUpdated != value))
				{
					this._LastUpdated = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Active", DbType="Bit NOT NULL")]
		[global::System.Runtime.Serialization.DataMemberAttribute(Order=6)]
		public bool Active
		{
			get
			{
				return this._Active;
			}
			set
			{
				if ((this._Active != value))
				{
					this._Active = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_FirstUpdated", DbType="DateTime")]
		[global::System.Runtime.Serialization.DataMemberAttribute(Order=7)]
		public System.Nullable<System.DateTime> FirstUpdated
		{
			get
			{
				return this._FirstUpdated;
			}
			set
			{
				if ((this._FirstUpdated != value))
				{
					this._FirstUpdated = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Culture", DbType="NChar(10)")]
		[global::System.Runtime.Serialization.DataMemberAttribute(Order=8)]
		public string Culture
		{
			get
			{
				return this._Culture;
			}
			set
			{
				if ((this._Culture != value))
				{
					this._Culture = value;
				}
			}
		}
	}
}
#pragma warning restore 1591