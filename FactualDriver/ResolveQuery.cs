﻿using System;
using System.Collections.Generic;
using FactualDriver.Filters;
using Newtonsoft.Json;

namespace FactualDriver
{
    public class ResolveQuery
    {
        private Dictionary<string, object> _parameters = new Dictionary<string, object>();

        public ResolveQuery Add(string key, object value)
        {
            if (value is double)
                value = string.Format("{0:N2}", Convert.ToDouble(value));

            if (_parameters.ContainsKey(key))
            {
                _parameters[key] = value;
            }
            else
            {
                _parameters.Add(key, value);
            }
            return this;
        }


        public string ToUrlQuery()
        {
            return string.Format("{0}={1}", Constants.RESOLVE_VALUES, JsonConvert.SerializeObject(_parameters));
        }
    }
}