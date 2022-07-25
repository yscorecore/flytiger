---
layout: default
title: AutoNotify
nav_order: 5
parent: FEATURES
---

## AutoNotify


`FlyTiger.AutoNotifyAttribute` will help us generate property with changed event. 

- No Use FlyTiger
    ```csharp
    public class Service1: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int _value;

        public int Value
        {
            get
            {
                return this._value;
            }
            set
            {
                if(this._value != value)
                {
                    this._value = value;
                    this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
                }
            }
        }
    }
    ```
- Use FlyTiger
    ```csharp
    public partial class Service1
    {
        [AutoNotify]
        private int _value;
    }
    ```
