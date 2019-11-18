﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ChatClient.ServiceChat {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ServerUser", Namespace="http://schemas.datacontract.org/2004/07/WcfChat")]
    [System.SerializableAttribute()]
    public partial class ServerUser : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private ChatClient.ServiceChat.NameColor ColorField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string NameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int RoomField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public ChatClient.ServiceChat.NameColor Color {
            get {
                return this.ColorField;
            }
            set {
                if ((this.ColorField.Equals(value) != true)) {
                    this.ColorField = value;
                    this.RaisePropertyChanged("Color");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Name {
            get {
                return this.NameField;
            }
            set {
                if ((object.ReferenceEquals(this.NameField, value) != true)) {
                    this.NameField = value;
                    this.RaisePropertyChanged("Name");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int Room {
            get {
                return this.RoomField;
            }
            set {
                if ((this.RoomField.Equals(value) != true)) {
                    this.RoomField = value;
                    this.RaisePropertyChanged("Room");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="NameColor", Namespace="http://schemas.datacontract.org/2004/07/WcfChat")]
    public enum NameColor : int {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Black = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Green = 1,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Brue = 2,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Red = 3,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Yellow = 4,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Gray = 5,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Purple = 6,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Pink = 7,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Orange = 8,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Brown = 9,
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServiceChat.IServiceChat", CallbackContract=typeof(ChatClient.ServiceChat.IServiceChatCallback))]
    public interface IServiceChat {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceChat/Connect", ReplyAction="http://tempuri.org/IServiceChat/ConnectResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(System.Exception), Action="http://tempuri.org/IServiceChat/ConnectExceptionFault", Name="Exception", Namespace="http://schemas.datacontract.org/2004/07/System")]
        int Connect(ChatClient.ServiceChat.ServerUser user);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceChat/Connect", ReplyAction="http://tempuri.org/IServiceChat/ConnectResponse")]
        System.Threading.Tasks.Task<int> ConnectAsync(ChatClient.ServiceChat.ServerUser user);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceChat/Disconnect", ReplyAction="http://tempuri.org/IServiceChat/DisconnectResponse")]
        void Disconnect(int id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceChat/Disconnect", ReplyAction="http://tempuri.org/IServiceChat/DisconnectResponse")]
        System.Threading.Tasks.Task DisconnectAsync(int id);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IServiceChat/SendMsg")]
        void SendMsg(string msg, int id, int room);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IServiceChat/SendMsg")]
        System.Threading.Tasks.Task SendMsgAsync(string msg, int id, int room);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceChat/ChangeRoom", ReplyAction="http://tempuri.org/IServiceChat/ChangeRoomResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(System.Exception), Action="http://tempuri.org/IServiceChat/ChangeRoomExceptionFault", Name="Exception", Namespace="http://schemas.datacontract.org/2004/07/System")]
        void ChangeRoom(int id, int room);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceChat/ChangeRoom", ReplyAction="http://tempuri.org/IServiceChat/ChangeRoomResponse")]
        System.Threading.Tasks.Task ChangeRoomAsync(int id, int room);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceChat/ChangeColor", ReplyAction="http://tempuri.org/IServiceChat/ChangeColorResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(System.Exception), Action="http://tempuri.org/IServiceChat/ChangeColorExceptionFault", Name="Exception", Namespace="http://schemas.datacontract.org/2004/07/System")]
        void ChangeColor(int id, int color);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceChat/ChangeColor", ReplyAction="http://tempuri.org/IServiceChat/ChangeColorResponse")]
        System.Threading.Tasks.Task ChangeColorAsync(int id, int color);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceChat/GetAllMessages", ReplyAction="http://tempuri.org/IServiceChat/GetAllMessagesResponse")]
        string[] GetAllMessages();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceChat/GetAllMessages", ReplyAction="http://tempuri.org/IServiceChat/GetAllMessagesResponse")]
        System.Threading.Tasks.Task<string[]> GetAllMessagesAsync();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IServiceChatCallback {
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IServiceChat/MsgCallBack")]
        void MsgCallBack(string msg, int color);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IServiceChatChannel : ChatClient.ServiceChat.IServiceChat, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ServiceChatClient : System.ServiceModel.DuplexClientBase<ChatClient.ServiceChat.IServiceChat>, ChatClient.ServiceChat.IServiceChat {
        
        public ServiceChatClient(System.ServiceModel.InstanceContext callbackInstance) : 
                base(callbackInstance) {
        }
        
        public ServiceChatClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName) : 
                base(callbackInstance, endpointConfigurationName) {
        }
        
        public ServiceChatClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, string remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public ServiceChatClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public ServiceChatClient(System.ServiceModel.InstanceContext callbackInstance, System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, binding, remoteAddress) {
        }
        
        public int Connect(ChatClient.ServiceChat.ServerUser user) {
            return base.Channel.Connect(user);
        }
        
        public System.Threading.Tasks.Task<int> ConnectAsync(ChatClient.ServiceChat.ServerUser user) {
            return base.Channel.ConnectAsync(user);
        }
        
        public void Disconnect(int id) {
            base.Channel.Disconnect(id);
        }
        
        public System.Threading.Tasks.Task DisconnectAsync(int id) {
            return base.Channel.DisconnectAsync(id);
        }
        
        public void SendMsg(string msg, int id, int room) {
            base.Channel.SendMsg(msg, id, room);
        }
        
        public System.Threading.Tasks.Task SendMsgAsync(string msg, int id, int room) {
            return base.Channel.SendMsgAsync(msg, id, room);
        }
        
        public void ChangeRoom(int id, int room) {
            base.Channel.ChangeRoom(id, room);
        }
        
        public System.Threading.Tasks.Task ChangeRoomAsync(int id, int room) {
            return base.Channel.ChangeRoomAsync(id, room);
        }
        
        public void ChangeColor(int id, int color) {
            base.Channel.ChangeColor(id, color);
        }
        
        public System.Threading.Tasks.Task ChangeColorAsync(int id, int color) {
            return base.Channel.ChangeColorAsync(id, color);
        }
        
        public string[] GetAllMessages() {
            return base.Channel.GetAllMessages();
        }
        
        public System.Threading.Tasks.Task<string[]> GetAllMessagesAsync() {
            return base.Channel.GetAllMessagesAsync();
        }
    }
}
