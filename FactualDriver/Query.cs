﻿using FactualDriver.Filters;
using FactualDriver.Utils;
using System;
using System.Collections.Generic;

namespace FactualDriver
{
    /// <summary>
    /// Represents a top level Factual query. Knows how to represent the query as URL
    /// encoded key value pairs, ready for the query string in a GET request.
    /// </summary>
    public class Query : IQuery
    {
        /// <summary>
        /// Holds all parameters for this query
        /// </summary>
        private Parameters _parameters = new Parameters();

        /// <summary>
        /// Sets the maximum amount of records to return from this Query.
        /// </summary>
        /// <param name="limit">limit the maximum amount of records to return from this Query.</param>
        /// <returns>this Query</returns>
        public Query Limit(long limit)
        {
            AddFilter(new Filter(Constants.QUERY_LIMIT, limit));
            return this;
        }

        /// <summary>
        /// Sets a full text search query. Factual will use this value to perform a
        /// full text search against various attributes of the underlying table, such
        /// as entity name, address, etc.
        /// </summary>
        /// <param name="term">the text for which to perform a full text search.</param>
        /// <returns>this Query</returns>
        public Query Search(string term)
        {
            AddFilter(new Filter(Constants.SEARCH, term));
            return this;
        }

        /// <summary>
        /// Sets a exact text search query. Factual will use this value to perform a
        /// exact text search against various attributes of the underlying table, such
        /// as entity name, address, etc.
        /// </summary>
        /// <param name="term">the text for which to perform a full text search.</param>
        /// <returns>this Query</returns>
        public Query SearchExact(string term)
        {
            string newTerm = string.Format("{0}{1}{2}", Constants.QUOTES, term, Constants.QUOTES);
            AddFilter(new Filter(Constants.SEARCH, newTerm));
            return this;
        }

        /// <summary>
        /// Adds a filter so that results can only be (roughly) within the specified
        /// geographic circle.
        /// </summary>
        /// <param name="circle">The circle within which to bound the results.</param>
        /// <returns>this Query.</returns>
        public Query WithIn(Circle circle)
        {
            AddFilter(circle.GetFilter());
            return this;
        }

        /// <summary>
        /// Adds a filter so that results can only be (roughly) within the specified
        /// geographic rectangle.
        /// </summary>
        /// <param name="rect">The rectangle within which to bound the results.</param>
        /// <returns>this Query.</returns>
        public Query WithIn(Rectangle rect)
        {
            AddFilter(rect);
            return this;
        }

        /// <summary>
        /// Sets this Query to sort field in ascending order.
        /// </summary>
        /// <param name="field">the field to sort in ascending order.</param>
        /// <returns>this Query</returns>
        public Query SortAsc(string field)
        {
            _parameters.AddCommaSeparatedFilter(Constants.QUERY_SORT, field + ":asc");
            return this;
        }

        /// <summary>
        /// Sets this Query to sort field in descending order.
        /// </summary>
        /// <param name="field">field to sort in ascending order.</param>
        /// <returns>this Query</returns>
        public Query SortDesc(string field)
        {
            _parameters.AddCommaSeparatedFilter(Constants.QUERY_SORT, field + ":desc");
            return this;
        }

        /// <summary>
        /// Sets this Query to perform a blended sort on rank and distance with the specified weights.
        /// This will only have an effect if a geo filter is used with this query.
        /// </summary>
        /// <param name="rankWeight">weight for placerank.</param>
        /// <param name="distanceWeight">weight for distance.</param>
        /// <returns>this Query</returns>
        public Query SortBlendRankAndDistance(int rankWeight, int distanceWeight)
        {
            _parameters.AddCommaSeparatedFilter(Constants.QUERY_SORT, "{\"placerank\":" + rankWeight + ",\"distance\":" + distanceWeight + "}");
            return this;
        }

        /// <summary>
        /// Sets the fields to select. This is optional; default behaviour is generally
        /// to select all fields in the schema.
        /// </summary>
        /// <param name="fields">the fields to select.</param>
        /// <returns>this Query</returns>
        public Query Only(IEnumerable<string> fields)
        {
            foreach (var field in fields)
            {
                _parameters.AddCommaSeparatedFilter(Constants.QUERY_SELECT, field);
            }
            return this;
        }


        /// <summary>
        /// Sets the fields to select. This is optional; default behaviour is generally
        /// to select all fields in the schema.
        /// </summary>
        /// <param name="fields">the fields to select.</param>
        /// <returns>this Query</returns>
        public Query Only(params string[] fields)
        {
            return Only((IEnumerable<string>)fields);
        }

        /// <summary>
        /// Sets how many records in to start getting results (i.e., the page offset)
        /// </summary>
        /// <param name="offset">for this Query.</param>
        /// <returns>this Query</returns>
        public Query Offset(int offset)
        {
            _parameters.Add(Constants.QUERY_OFFSET, offset);
            return this;
        }

        /// <summary>
        /// The response will include a count of the total number of rows in the table
        /// that conform to the request based on included filters. This will increase
        /// the time required to return a response. The default behavior is to NOT
        /// include a row count.
        /// </summary>
        /// <returns>this Query, marked to return total row count when run.</returns>
        public Query IncludeRowCount()
        {
            return IncludeRowCount(true);
        }

        /// <summary>
        /// When true, the response will include a count of the total number of rows in
        /// the table that conform to the request based on included filters.
        /// Requesting the row count will increase the time required to return a
        /// response. The default behavior is to NOT include a row count.
        /// </summary>
        /// <param name="includeRowCount">
        /// true if you want the results to include a count of the total
        /// number of rows in the table that conform to the request based on
        /// included filters.
        /// </param>
        /// <returns>this Query, marked to return total row count when run.</returns>
        public Query IncludeRowCount(bool includeRowCount)
        {
            _parameters.Add(Constants.INCLUDE_COUNT, includeRowCount);
            return this;
        }

        /// <summary>
        ///  Chooses an existence threshold. For on the latest enumuration of values
        ///  that can be passed into the threshold value, please see the Read API
        ///  documentation: http://developer.factual.com/api-docs/#Read, in particular
        ///  the threshold parameter.
        /// </summary>
        /// <param name="threshold">See Read API documentation for potential values</param>
        /// <returns></returns>
        public Query Threshold(string threshold)
        {
            _parameters.Add(Constants.QUERY_THRESHOLD, threshold);
            return this;
        }

        /// <summary>
        /// Begins construction of a new row filter for this Query.
        /// </summary>
        /// <param name="fieldName">the name of the field on which to filter.</param>
        /// <returns>A partial representation of the new row filter.</returns>
        public QueryBuilder<Query> Field(string fieldName)
        {
            return new QueryBuilder<Query>(this, fieldName);
        }

        /// <summary>
        /// Converts a path object into a uri path string for communication with
        /// Factual's API. Provides proper URL encoding and escaping.
        /// </summary>
        /// <returns>Returns the path string to represent this Query when talking to Factual's API.</returns>
        public string ToUrlQuery()
        {
            return JsonUtil.ToQueryString(_parameters.ToFilterArray());
        }

        /// <summary>
        /// Adds filter to this Query.
        /// </summary>
        /// <param name="filter"></param>
        [Obsolete("please use AddFilter() instead")]
        public void Add(IFilter filter)
        {
            AddFilter(filter);
        }

        /// <summary>
        /// Adds filter to this Query.
        /// </summary>
        /// <param name="filter"></param>
        public Query AddFilter(IFilter filter)
        {
            _parameters.Add(filter);
            return this;
        }

        /// <summary>
        /// Used to nest AND'ed predicates.
        /// </summary>
        /// <param name="queries"></param>
        /// <returns></returns>
        public Query And(IEnumerable<Query> queries)
        {
            _parameters.PopRowFiltersIntoNewGroup(Constants.FILTER_AND, queries);
            return this;
        }

        /// <summary>
        /// Used to nest AND'ed predicates.
        /// </summary>
        /// <param name="queries"></param>
        /// <returns></returns>
        public Query And(params Query[] queries)
        {
            return And((IEnumerable<Query>)queries);
        }

        /// <summary>
        /// Used to nest OR'ed predicates.
        /// </summary>
        /// <param name="queries"></param>
        /// <returns></returns>
        public Query Or(IEnumerable<Query> queries)
        {
            _parameters.PopRowFiltersIntoNewGroup(Constants.FILTER_OR, queries);
            return this;
        }

        /// <summary>
        /// Used to nest OR'ed predicates.
        /// </summary>
        /// <param name="queries"></param>
        /// <returns></returns>
        public Query Or(params Query[] queries)
        {
            return Or((IEnumerable<Query>)queries);
        }
    }
}
