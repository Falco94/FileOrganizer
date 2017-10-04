using MahApps.Metro.Controls;
using Runtime.MVC;
using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Input;

namespace FileOrganizer.MVC
{
    public class BaseMahappsWindow : MetroWindow
    {
            private ModelBase _model;
            private ControllerBase _controller;

            public event PropertyChangedEventHandler PropertyChanged;

            /// <summary>
            /// The basic constructor
            /// </summary>
            public BaseMahappsWindow()
            {
                Loaded += OnWindowLoaded;
            }

            public ModelBase Model
            {
                get
                {
                    return _model;
                }
                set
                {
                    _model = value;

                    if (_model != null)
                    {
                        DataContext = _model;
                    }

                    OnPropertyChanged("Model");
                }
            }

            public ControllerBase Controller
            {
                get
                {
                    return _controller;
                }
                set
                {
                    _controller = value;

                    if (_controller != null)
                    {
                        var type = _controller.GetType();

                        var fieldInfos =
                            type.GetFields(BindingFlags.FlattenHierarchy | BindingFlags.GetField | BindingFlags.Public |
                                           BindingFlags.Static);

                        foreach (var fieldInfo in fieldInfos)
                        {
                            if (fieldInfo.FieldType.Equals(typeof(RoutedCommand)) ||
                                fieldInfo.FieldType.IsSubclassOf(typeof(RoutedCommand)))
                            {
                                var command = fieldInfo.GetValue(null) as RoutedCommand;

                                if (command != null)
                                {
                                    ConnectControllerCommand(command);
                                }
                            }
                        }
                    }

                    OnPropertyChanged("Controller");
                }
            }

            private void ConnectControllerCommand(RoutedCommand command)
            {
                var methodExecutedInfo = _controller.GetType()
                    .GetMethod("Command" + command.Name,
                        BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Public |
                        BindingFlags.IgnoreCase);
                var methodCanExecuteInfo = _controller.GetType()
                    .GetMethod("CheckCommand" + command.Name,
                        BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Public |
                        BindingFlags.IgnoreCase);

                if (methodExecutedInfo != null && methodCanExecuteInfo != null)
                {
                    var binding = new CommandBinding();

                    binding.Command = command;

                    var executedEvent = binding.GetType().GetEvent("Executed");
                    var canExecuteEvent = binding.GetType().GetEvent("CanExecute");

                    var delegateExecutedType = executedEvent.EventHandlerType;
                    var delegateCanExecuteType = canExecuteEvent.EventHandlerType;

                    var executed = Delegate.CreateDelegate(delegateExecutedType, _controller, methodExecutedInfo);
                    var canExecute = Delegate.CreateDelegate(delegateCanExecuteType, _controller,
                        methodCanExecuteInfo);

                    var addExecutedHandler = executedEvent.GetAddMethod();
                    object[] addExecutedHandlerArgs = { executed };

                    var addCanExecuteHandler = canExecuteEvent.GetAddMethod();
                    object[] addCanExecuteHandlerArgs = { canExecute };

                    addExecutedHandler.Invoke(binding, addExecutedHandlerArgs);
                    addCanExecuteHandler.Invoke(binding, addCanExecuteHandlerArgs);

                    CommandBindings.Add(binding);
                }
            }

            protected virtual void OnWindowLoaded(object sender, RoutedEventArgs args)
            {
                Loaded -= OnWindowLoaded;
            }

            protected void OnPropertyChanged(string propertyName)
            {
                if (PropertyChanged != null)
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

