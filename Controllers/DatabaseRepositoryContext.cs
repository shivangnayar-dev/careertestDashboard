using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics.Metrics;
using System.Dynamic;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;

//using System.Web.Configuration;
using System.Web.Mvc;
//using System.Web.UI;
using System.Xml.Linq;
using Auxx;
using Auxx.Models;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
using Org.BouncyCastle.Asn1.Pkcs;
using static Google.Protobuf.Reflection.SourceCodeInfo.Types;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

[Authorize]
public class DatabaseRepositorycContext
{
    // private readonly ApplicationDbContext _configuration;
    string _orgName = string.Empty;
    string _roleName = string.Empty;
    private readonly string _connectionString;
    private const string startdashboardfromdate = "DATE_SUB(CURRENT_DATE(), INTERVAL 30 DAY)";

    public DatabaseRepositorycContext(IConfiguration configuration, string orgName, string rolename)
    //public DatabaseRepositorycContext(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
        _orgName = orgName;// HttpContext.User.FindFirst("OrganizationName")?.Value;
        _roleName = rolename;
    }

    //gets the values for donut chart viz., Register, Test and Completed for a selected organization from validation table
    public List<int> GetTestStageDBData()
    {
        var listData = new List<int>();
        try
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var query2 = string.Empty;
                var query = @" SELECT ";
                if (_roleName != "CG Superadmin")
                {
                    query = query + " organisation, ";
                }
                query = query + " SUM(CASE WHEN register_page = 1 THEN 1 ELSE 0 END) AS register, " +
                        //SUM(CASE WHEN test_section_1 = 1 OR test_setion_2 = 1 OR test_section_3 = 1 OR test_section_4 = 1 OR test_section_5 = 1 THEN 1 ELSE 0 END) AS test, " +
                        "(SELECT SUM(CASE WHEN StartTime >= " + startdashboardfromdate + " AND StartTime < CURRENT_DATE() + INTERVAL 1 DAY THEN 1 ELSE 0 END) " +
                        " FROM cgstagingdata ";
                if (_roleName != "CG Superadmin")
                {
                    query = query + " WHERE organization  = '" + _orgName + "'";
                }
                    query = query + ") as test, " +
                        "(SELECT SUM(CASE WHEN StartTime >= " + startdashboardfromdate + " AND StartTime < CURRENT_DATE() + INTERVAL 1 DAY THEN 1 ELSE 0 END) " +
                        " FROM cgstagingdata WHERE reporturl IS NOT NULL " +
                        " AND reporturl != '' ";
                if (_roleName != "CG Superadmin")
                {
                    query = query + " AND organization  = '" + _orgName + "'";
                }
                query = query + " ) as completed " +
                    " FROM validation_table ";
                query = query + "WHERE " +
                    //"MONTH(timestamp) = MONTH(CURRENT_DATE()) AND YEAR(timestamp) = YEAR(CURRENT_DATE())";
                    "timestamp BETWEEN " + startdashboardfromdate + " AND CURDATE() ";

                if (_roleName != "CG Superadmin")
                {
                    query2 = " AND organisation = '" + _orgName + "'" + " GROUP BY organisation";
                }
                query = query + query2;

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@orgName", _orgName);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            listData.Add(reader.GetInt32("register"));
                            listData.Add(reader.GetInt32("test"));
                            listData.Add(reader.GetInt32("completed"));
                        }
                    }
                }
            }

            return listData;
        }catch
        { }
        return listData;
    }

    //gets the values for donut chart viz., test attempts for a selected organization from testinfo table
    public List<int> GetTestAttemptsDBData()
    {
        var listData = new List<int>();
        try
        {
            List<int> monthCountIsOne = new List<int>();
            List<int> monthCountMoreThanOne = new List<int>();

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var query2 = string.Empty;
                /* var query = "SELECT TestCode, Organisation, count(TestCode) as attempts " +
                     "FROM TestInfo ";
                 query = query + "WHERE " +
                     "test_created BETWEEN '"+ startdashboardfromdate +"' AND CURDATE() ";
                 //"MONTH(test_created) = MONTH(CURRENT_DATE()) AND YEAR(test_created) = YEAR(CURRENT_DATE())";
                 if (_roleName != "CG Superadmin")
                 {
                     query2 = " AND organisation = '" + _orgName + "' ";
                 }
                 query = query + query2 + " GROUP BY testcode, organisation";*/

                var spname = "sp_GetTestAttemptsCounts";

                using (var command = new MySqlCommand(spname, connection))
                {
                    // Specify that the command is a stored procedure
                    command.CommandType = CommandType.StoredProcedure;

                    // Add input parameters (if any)
                    command.Parameters.AddWithValue("@orgname", _orgName);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int monthCount = Convert.ToInt32(reader["monthCount"]);
                            int todayCount = Convert.ToInt32(reader["todayCount"]);

                            // Append the values to the respective lists
                            if (monthCount == 1)
                            {
                                monthCountIsOne.Add(monthCount);
                            }
                            else if (monthCount > 1)
                            {
                                monthCountMoreThanOne.Add(monthCount);
                            }
                        }
                    }
                }
                // Convert lists to integer arrays
                int[] monthCountOneArray = monthCountIsOne.ToArray();
                int[] monthCountMoreThanOneArray = monthCountMoreThanOne.ToArray();

                listData.Add(monthCountIsOne.Sum());
                listData.Add(monthCountMoreThanOne.Sum());

            }
            return listData;
        }
        catch { }

        return listData;

    }

    public List<int> GetSumOfTestsDBData(string orgName)
    {
        var listData = new List<int>();

        using (var connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            var query2 = string.Empty;
            var query = @"
                SELECT 
                    COUNT(candidateid) AS total_current_month_count,
                    SUM(CASE WHEN DATE(timestamp) = CURRENT_DATE() THEN 1 ELSE 0 END) AS today_count
                FROM validation_table
                WHERE " +
                "timestamp BETWEEN " + startdashboardfromdate + " AND CURDATE() " +
            //MONTH(timestamp) = MONTH(CURRENT_DATE()) 
            //AND YEAR(timestamp) = YEAR(CURRENT_DATE())
               "AND (test_section_1 = 1 " +
                "OR test_section_2 = 1  " +
                "OR test_section_3 = 1  " +
                "OR test_section_4 = 1  " +
                "OR test_section_5 = 1) ";
            if (_roleName != "CG Superadmin")
            {
                query2 = " AND organisation = '" + _orgName + "' ";
            }
            query = query + query2;

            using (var command = new MySqlCommand(query, connection))
            {
                // Parameterize the query to prevent SQL injection
                //command.Parameters.AddWithValue("@orgName", orgName);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Add query results to the list
                        listData.Add(reader.IsDBNull(0) ? 0 : reader.GetInt32(0)); // total_current_month_count
                        listData.Add(reader.IsDBNull(1) ? 0 : reader.GetInt32(1)); // today_count
                    }
                }
            }
        }

        return listData;
    }

    public List<int> GetSumOfLoginsDBData()
    {
        var listData = new List<int>();

        using (var connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            var query2 = string.Empty;
            var query = @"
                    SELECT  COUNT(login_page) AS total_current_month_count, SUM(CASE WHEN DATE(timestamp) = (CURRENT_DATE()) THEN 1 ELSE 0 END) AS today_count " +
                    "FROM validation_table WHERE " +
                    //"MONTH(timestamp) = MONTH(CURRENT_DATE()) AND YEAR(timestamp) = YEAR(CURRENT_DATE()) " +
                    "timestamp BETWEEN " + startdashboardfromdate + " AND CURDATE() " +
                    "AND login_page = 1 ";
            // query = query + " WHERE MONTH(timestamp) = MONTH(CURRENT_DATE()) AND YEAR(timestamp) = YEAR(CURRENT_DATE()) ";

            if (_roleName != "CG Superadmin")
            {
                query2 = " AND organisation = '" + _orgName + "'";
            }
            query = query + query2;
            using (var command = new MySqlCommand(query, connection))
            {
                // Parameterize the query to prevent SQL injection
                //command.Parameters.AddWithValue("@orgName", _orgName);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Add query results to the list
                        listData.Add(reader.IsDBNull(0) ? 0 : reader.GetInt32(0)); // total_current_month_count
                        listData.Add(reader.IsDBNull(1) ? 0 : reader.GetInt32(1)); // today_count
                    }
                }
            }
        }
        return listData;
    }

    public List<int> GetSumOfCompletedDBData()
    {
        var listData = new List<int>();

        using (var connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            var query2 = string.Empty;
            /*var query = @"
                     SELECT  COUNT(candidateid) AS total_current_month_count, SUM(CASE WHEN DATE(timestamp) = (CURRENT_DATE()) THEN 1 ELSE 0 END) AS today_count " +
                    "FROM validation_table " +
                    "WHERE " +
                    //"MONTH(timestamp) = MONTH(CURRENT_DATE()) AND YEAR(timestamp) = YEAR(CURRENT_DATE()) " +
                    "timestamp BETWEEN '" + startdashboardfromdate + "' AND CURDATE() " +
                    "AND teststatus = 'completed' "; */
            var query = @"
                        SELECT 
	                        SUM(CASE 
		                        WHEN StartTime >= " + startdashboardfromdate + 
                                " AND StartTime < CURRENT_DATE() + INTERVAL 1 DAY THEN 1 ELSE 0 END) AS total_current_month_count," +
	                        " SUM(CASE WHEN DATE(StartTime) = CURRENT_DATE() THEN 1 ELSE 0 END) AS today_count " +
	                       " FROM cgstagingdata " +
	                       " WHERE reporturl IS NOT NULL AND reporturl != '' ";

            if (_roleName != "CG Superadmin")
            {
                query2 = " AND organisation = '" + _orgName + "'";
            }
            query = query + query2;
            using (var command = new MySqlCommand(query, connection))
            {
                // Parameterize the query to prevent SQL injection
                // command.Parameters.AddWithValue("@orgName", _orgName);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Add query results to the list
                        listData.Add(reader.IsDBNull(0) ? 0 : reader.GetInt32(0)); // total_current_month_count
                        listData.Add(reader.IsDBNull(1) ? 0 : reader.GetInt32(1)); // today_count
                    }
                }
            }
        }
        return listData;
    }

    public List<int> GetSumOfUsersDBData()
    {
        var listData = new List<int>();

        using (var connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            var query2 = string.Empty;
            var query = @"SELECT  COUNT(candidateid) AS total_current_month_count, SUM(CASE WHEN DATE(timestamp) = (CURRENT_DATE()) THEN 1 ELSE 0 END) AS today_count " +
                    "FROM validation_table " +
                    "WHERE " +
                    //"MONTH(timestamp) = MONTH(CURRENT_DATE()) AND YEAR(timestamp) = YEAR(CURRENT_DATE()) " +
                    "timestamp BETWEEN '" + startdashboardfromdate + "' AND CURDATE() " +
                    "AND register_page = 1' ";
            if (_roleName != "CG Superadmin")
            {
                query2 = " AND organisation = '" + _orgName + "'";
            }
            query = query + query2;
            using (var command = new MySqlCommand(query, connection))
            {
                // Parameterize the query to prevent SQL injection
                //command.Parameters.AddWithValue("@orgName", _orgName);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Add query results to the list
                        listData.Add(reader.IsDBNull(0) ? 0 : reader.GetInt32(0)); // total_current_month_count
                        listData.Add(reader.IsDBNull(1) ? 0 : reader.GetInt32(1)); // today_count
                    }
                }
            }
        }
        return listData;
    }

    public List<int> GetDaywiseOfValidationDBData()
    {
        var listData = new List<int>();

        using (var connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            var query2 = string.Empty;
            var query = @"SELECT  DATE(timestamp) AS day, SUM(CASE WHEN login_page = 1 THEN 1 ELSE 0 END) AS newuser, SUM(CASE WHEN register_page = 1 THEN 1 ELSE 0 END) AS register, SUM(CASE WHEN test_section_1 = 1 OR test_section_2 = 1 OR test_section_3 = 1 OR test_section_4 = 1 OR test_section_5 = 1 THEN 1 ELSE 0 END) AS test, SUM(CASE WHEN teststatus = 'completed' THEN 1 ELSE 0 END) AS completed " +
                            "FROM validation_table ";
            query = query + " WHERE " +
            //    "MONTH(timestamp) = MONTH(CURRENT_DATE()) AND YEAR(timestamp) = YEAR(CURRENT_DATE()) ";
            "timestamp BETWEEN " + startdashboardfromdate + " AND CURDATE() ";

            if (_roleName != "CG Superadmin")
            {
                query2 = " WHERE organisation = '" + _orgName + "' ";
            }
            query = query + query2 + " GROUP BY DATE(timestamp) ORDER BY day";
            using (var command = new MySqlCommand(query, connection))
            {
                // Parameterize the query to prevent SQL injection
                //command.Parameters.AddWithValue("@orgName", _orgName);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Add query results to the list
                        listData.Add(reader.IsDBNull(0) ? 0 : reader.GetInt32(0)); // dates
                        listData.Add(reader.IsDBNull(1) ? 0 : reader.GetInt32(1)); // newuser
                        listData.Add(reader.IsDBNull(2) ? 0 : reader.GetInt32(2)); // register
                        listData.Add(reader.IsDBNull(3) ? 0 : reader.GetInt32(3)); // test
                        listData.Add(reader.IsDBNull(4) ? 0 : reader.GetInt32(4)); // completed
                    }
                }
            }
        }
        return listData;
    }

    // test summary table having testcode, #month, #day and expire date
    public List<object> GetTestSummaryDBData()
    {
        var listData = new List<object>();
        try
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var query2 = string.Empty;
                /* var query = @"SELECT TestCode, COUNT(TestCode) AS total_current_month_count,SUM(CASE WHEN DATE(test_created) = (CURRENT_DATE()) THEN 1 ELSE 0 END) AS today_count," +
                             "DATE_FORMAT(test_expire,'%Y-%m-%d') as test_expire " +
                             "FROM TestInfo " +
                             "WHERE " +
                             //"MONTH(test_created) = MONTH(CURRENT_DATE()) " +
                             //"AND YEAR(test_created) = YEAR(CURRENT_DATE()) ";
                             "test_created BETWEEN '" + startdashboardfromdate + "' AND CURDATE() ";*/

                var query = @" SELECT  
                            temp.TestCode as testcode,
                            SUM(CASE 
	                            WHEN StartTime >= " + startdashboardfromdate +
                                    " AND Starttime < CURRENT_DATE() + INTERVAL 1 DAY THEN 1 ELSE 0 END) AS total_current_month_count, " +
                                 " SUM(CASE WHEN DATE(Starttime) = CURRENT_DATE() THEN 1 ELSE 0 END) AS today_count, DATE(test_expire) as test_expire " +
                              " FROM cgstagingdata temp " +
                              " INNER JOIN TestInfo t on t.TestCode = temp.TestCode AND test_created IS NOT NULL AND test_expire IS NOT NULL " +
                               "WHERE Starttime >= " + startdashboardfromdate + " AND Starttime < CURRENT_DATE() ";

                if (_roleName != "CG Superadmin")
                {
                    query2 = " AND temp.organization = '" + _orgName + "'";
                }
                query = query + query2 + " GROUP BY TestCode, DATE(test_expire)";
                using (var command = new MySqlCommand(query, connection))
                {
                    // Parameterize the query to prevent SQL injection
                    // command.Parameters.AddWithValue("@orgName", _orgName);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            listData.Add(new
                            {
                                testcode = reader["testcode"],
                                total_current_month_count = reader["total_current_month_count"],
                                today_count = reader["today_count"],
                                test_expire = reader["test_expire"]
                            });
                        }
                    }
                }
            }
            return listData;
        }
        catch { 
        }
        return listData;

    }

    //get staging data for a selected testcode
    public List<object> GetTestSummaryDetailsDBData(string testcode)
    {
        var listData = new List<object>();
        try
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var query2 = string.Empty;
                var query = @"SELECT `CGStagId`,`CandidateId`,`TestCode`,`Starttime`,`Endtime`,`name`,`ReportName`, `teststatus`,`Mobile_No`, `reporturl`, " +
                        "Organization, `Report_to_candidate_1_pager`, `BenchMarkOrganisation`,  `top5Motivations1`, `top5Motivations2`, `top5Motivations3`,`age`,`email_address`,`gender`,`location`,`country`,`qualification`,`selectedSpecializations1`, `selectedSpecializations2`,`selectedSpecializations3`, " +
                        "`selectedIndustries1`,`selectedIndustries2`, `selectedIndustries3`,`Suggestedjobrole1`,`Suggestedjobrole2`,`Suggestedjobrole3`,`mathStats`,`science`,`govJobs`,`armedForcesJobs`,`coreStream`" +
                        " FROM `cgstagingdata`  ";
                if (_roleName != "CG Superadmin")
                {
                    query2 = " WHERE organization = '" + _orgName + "'" + " AND Testcode = '" + testcode + "'" +
                        " AND Starttime >= " + startdashboardfromdate + " AND Starttime < CURRENT_DATE() ";
                }
                query = query + query2;
                string secondQuery = "SELECT CandidateID, SubAttributeName, Grade, SubAttributeValue, GradeValue FROM staging_subattributes " +
                                "WHERE stagingid  = @StgId";
                var combinedResults = new Dictionary<int, (string Name, List<object> SecondQueryResults)>();

                using (var command = new MySqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        int i = 1;
                        while (reader.Read())
                        {
                            //step4: add to list staging data and sub attribute data
                            listData.Add(new
                            {
                                SlNo = i,
                                cgstagid = reader["CGStagId"],
                                candidateid = reader["CandidateId"],
                                testcode = reader["TestCode"],
                                starttime = reader["Starttime"],
                                endtime = reader["Endtime"],
                                name = reader["name"],
                                reportname = reader["ReportName"],
                                teststatus = reader["teststatus"],
                                mobileno = reader["Mobile_No"],
                                reporturl = reader["reporturl"].ToString() != string.Empty ? reader["reporturl"] : string.Empty,
                                organization = reader["Organization"],
                                report_to_candidate_1_pager = reader["Report_to_candidate_1_pager"],
                                BenchMarkOrganisation = reader["BenchMarkOrganisation"],
                                top5Motivations1 = reader["top5Motivations1"],
                                top5Motivations2 = reader["top5Motivations2"],
                                top5Motivations3 = reader["top5Motivations3"],
                                age = reader["age"],
                                emailaddress = reader["email_address"],
                                gender = reader["gender"],
                                location = reader["location"],
                                country = reader["country"],
                                qualification = reader["qualification"],
                                selectedspecializations1 = reader["selectedSpecializations1"],
                                selectedspecializations2 = reader["selectedSpecializations2"],
                                selectedspecializations3 = reader["selectedSpecializations3"],
                                selectedindustries1 = reader["selectedIndustries1"],
                                selectedindustries2 = reader["selectedIndustries2"],
                                selectedindustries3 = reader["selectedIndustries3"],
                                suggestedjobrole1 = reader["Suggestedjobrole1"],
                                suggestedjobrole2 = reader["Suggestedjobrole2"],
                                suggestedjobrole3 = reader["Suggestedjobrole3"],
                                mathStats = reader["mathStats"],
                                science = reader["science"],
                                govjobs = reader["govJobs"],
                                armedforcesfobs = reader["armedForcesJobs"],
                                corestream = reader["coreStream"],
                            });
                            i++;
                            combinedResults[Convert.ToInt32(reader["CGStagId"])] = (reader["TestCode"].ToString(), listData);
                        }
                    }
                }
                DataTable finalPivotTable = new DataTable();

                foreach (var id in combinedResults.Keys)
                {
                    DataTable rawData = new DataTable();
                    DataTable pivotTable = new DataTable();
                    
                    using (MySqlCommand cmd1 = new MySqlCommand(secondQuery, connection))
                    {
                        cmd1.Parameters.AddWithValue("@StgId", id);

                        using (MySqlDataReader reader1 = cmd1.ExecuteReader())
                        {
                            rawData.Load(reader1);
                            //}
                            // }
                            // Step 1: Identify dynamic columns
                            HashSet<string> dynamicColumns = new HashSet<string>();
                            foreach (DataRow row in rawData.Rows)
                            {
                                string subAttr = row["SubAttributeName"]?.ToString();
                                string grade = row["Grade"]?.ToString();

                                if (!string.IsNullOrEmpty(subAttr)) dynamicColumns.Add(subAttr);
                                if (!string.IsNullOrEmpty(grade)) dynamicColumns.Add(grade);
                            }

                            // Step 2: Create pivoted DataTable

                            pivotTable.Columns.Add("CandidateID", typeof(int));
                            if (finalPivotTable.Columns.Count <= 0)
                                if (!finalPivotTable.Columns.Contains("CandidateID"))
                                    // if (finalPivotTable.Columns[0].ColumnName != "CandidateID")
                                    finalPivotTable.Columns.Add("CandidateID", typeof(int));

                            foreach (var column in dynamicColumns)
                            {
                                pivotTable.Columns.Add(column, typeof(object));
                                if (finalPivotTable.Columns.Count != (dynamicColumns.Count + 1) && !finalPivotTable.Columns.Contains(column))
                                    finalPivotTable.Columns.Add(column, typeof(object));
                            }
                            // Step 3: Populate pivot data
                            foreach (var group in rawData.AsEnumerable().GroupBy(row => row.Field<string>("CandidateID")))
                            {
                                DataRow newRow = pivotTable.NewRow();
                                newRow["CandidateID"] = group.Key;
                                DataRow finalnewRow = finalPivotTable.NewRow();
                                finalnewRow["CandidateID"] = group.Key;
                                foreach (var row in group)
                                {
                                    string subAttr = row["SubAttributeName"]?.ToString();
                                    string grade = row["Grade"]?.ToString();

                                    if (!string.IsNullOrEmpty(subAttr))
                                    {
                                        newRow[subAttr] = row["SubAttributeValue"];
                                        finalnewRow[subAttr] = row["SubAttributeValue"];
                                    }
                                    if (!string.IsNullOrEmpty(grade))
                                    {
                                        newRow[grade] = row["GradeValue"];
                                        finalnewRow[grade] = row["GradeValue"];
                                    }
                                }
                                pivotTable.Rows.Add(newRow);
                                finalPivotTable.Rows.Add(finalnewRow);
                            }

                        }
                    }
                }

                var updatedListData = new List<object>();
                foreach (var item in listData)
                {
                    // Convert anonymous object to dictionary for easier manipulation
                    var itemDict = item.GetType().GetProperties()
                        .ToDictionary(prop => prop.Name, prop => prop.GetValue(item, null));
                    // Check if "candidateid" exists in the current item
                    if (!itemDict.ContainsKey("candidateid"))
                    {
                        Console.WriteLine("Item does not contain 'candidateid'. Skipping.");
                        continue; // Skip this item and move to the next iteration
                    }

                    // Get the unique identifier (e.g., candidateid) to match with pivotTable
                    var candidateId = itemDict["candidateid"];

                    // Find matching row in pivotTable
                    DataRow[] matchingRows = finalPivotTable.Select($"CandidateId = '{candidateId}'");

                    if (matchingRows.Length > 0)
                    {
                        // Assume one-to-one mapping; use the first matching row
                        DataRow matchedRow = matchingRows[0];

                        // Add new columns from pivotTable to the item dictionary
                        foreach (DataColumn column in finalPivotTable.Columns)
                        {
                            if (!itemDict.ContainsKey(column.ColumnName) && column.ColumnName != "CandidateId")
                            {
                                itemDict[column.ColumnName] = matchedRow[column];
                            }
                        }
                    }
                    dynamic expando = new ExpandoObject();
                    foreach (var kvp in itemDict)
                    {
                        ((IDictionary<string, object>)expando).Add(kvp.Key, kvp.Value);
                    }

                    // Add the updated dictionary back as an anonymous object
                    updatedListData.Add(expando);
                }
                listData = updatedListData;
            }
            return listData;
        }
        catch { }
        return listData;
    }

    // test summary table having testcode, #month, #day and expire date
    public List<object> GetTestPerdayMonthwiseDBData()
    {
        var data = new Dictionary<string, dynamic>(); // Holds day-wise data
        var allDays = new List<object>(); // Complete list of days for current month
        try
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var query2 = string.Empty;
                var query = @"SELECT 
                            DATE(timestamp) AS day,
                            SUM(CASE WHEN login_page = 1 THEN 1 ELSE 0 END) AS newuser,
                            SUM(CASE WHEN register_page = 1 THEN 1 ELSE 0 END) AS register," +
                                //SUM(CASE WHEN test_section_1 = 1 OR test_section_2 = 1 OR test_section_3 = 1 OR test_section_4 = 1 OR test_section_5 = 1 THEN 1 ELSE 0 END) AS test,
                                //SUM(CASE WHEN teststatus = 'completed' THEN 1 ELSE 0 END) AS completed
                                "(SELECT SUM(CASE WHEN StartTime >= DATE(day) AND StartTime < DATE(day) +INTERVAL 1 DAY THEN 1 ELSE 0 END) " +
                                    " FROM cgstagingdata WHERE " +
                                    "  organization  = '" + _orgName + "') as test, " +
                                "(SELECT SUM(CASE WHEN StartTime >= DATE(day) AND StartTime < DATE(day) + INTERVAL 1 DAY THEN 1 ELSE 0 END) " +
                                    " FROM cgstagingdata  WHERE reporturl IS NOT NULL " +
                                " AND reporturl != '' " +
                                " AND organization  = '" + _orgName + "') as completed " +
                            " FROM validation_table " +
                            " WHERE " +
                //MONTH(timestamp) = MONTH(CURRENT_DATE())
                //AND YEAR(timestamp) = YEAR(CURRENT_DATE()) ";
                "timestamp BETWEEN " + startdashboardfromdate + " AND CURDATE() ";
                if (_roleName != "CG Superadmin")
                {
                    query2 = " AND organisation = '" + _orgName + "'";
                }
                query = query + query2 + " GROUP BY DATE(timestamp) ORDER BY day";

                using (var command = new MySqlCommand(query, connection))
                {
                    // Parameterize the query to prevent SQL injection
                    //command.Parameters.AddWithValue("@orgName", _orgName);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Store data for existing rows
                            string day = Convert.ToDateTime(reader["day"]).ToString("yyyy-MM-dd");
                            data[day] = new
                            {
                                newuser = Convert.ToInt32(reader["newuser"]),
                                register = Convert.ToInt32(reader["register"]),
                                test = Convert.ToInt32(reader["test"]),
                                completed = Convert.ToInt32(reader["completed"])
                            };
                        }
                    }
                }
            }

            // Generate all dates for the current month
            DateTime today = DateTime.Today;
            int daysInMonth = DateTime.DaysInMonth(today.Year, today.Month);

            for (int i = 1; i <= daysInMonth; i++)
            {
                string currentDate = new DateTime(today.Year, today.Month, i).ToString("yyyy-MM-dd");

                // Add missing dates with zero values
                if (!data.ContainsKey(currentDate))
                {
                    data[currentDate] = new { newuser = 0, register = 0, test = 0, completed = 0 };
                }
            }

            // Prepare a sorted list to ensure chronological order
            foreach (var day in data.OrderBy(x => x.Key))
            {
                allDays.Add(new
                {
                    day = day.Key,
                    newuser = day.Value.newuser,
                    register = day.Value.register,
                    test = day.Value.test,
                    completed = day.Value.completed
                });
            }

            return allDays;
        }
        catch { }
        return allDays;
    }

    public int[] GetTestUserCounts()
    {
        int[] cntTest = new int[2];
        try
        {
            int monthCount = 0;
            int todayCount = 0;
            //var listData = new List<object>();

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var query2 = string.Empty;

                var query = @" SELECT 
                                SUM(CASE WHEN StartTime >= DATE_SUB(CURRENT_DATE(), INTERVAL 30 DAY)
                                        AND StartTime < CURRENT_DATE() + INTERVAL 1 DAY 
                                        THEN 1 ELSE 0 END) AS monthCount,
                                SUM(CASE WHEN DATE(StartTime) = CURRENT_DATE() 
                                        THEN 1 ELSE 0 END) AS todayCount
                            FROM cgstagingdata " +
                                " WHERE ";

                if (_roleName != "CG Superadmin")
                {
                    query2 = "  organization = '" + _orgName + "'";
                }
                query = query + query2;

                using (var command = new MySqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            monthCount = Convert.ToInt32(reader["monthCount"] == DBNull.Value ? 0 : reader["monthCount"]);
                            todayCount = Convert.ToInt32(reader["todayCount"] == DBNull.Value ? 0 : reader["todayCount"]);
                        }
                    }
                }
            }
            cntTest[0] = monthCount;
            cntTest[1] = todayCount;

            return cntTest;
        }
        catch
        { }
        return cntTest ;
    }

    public int[] GetCompletedUserCounts()
    {
        int[] cntTest = new int[2];
        try
        {
            int monthCount = 0;
            int todayCount = 0;

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var query2 = string.Empty;

                /*var query = @"SELECT 
                                SUM(CASE 
                                    WHEN timestamp >= '2024-11-20' AND timestamp < CURRENT_DATE() + INTERVAL 1 DAY 
                                    AND MONTH(timestamp) = MONTH(CURRENT_DATE()) 
                                    AND YEAR(timestamp) = YEAR(CURRENT_DATE()) 
                                    THEN 1 ELSE 0 
                                END) AS monthCount,
                                SUM(CASE 
                                    WHEN timestamp >= '2024-11-20' AND timestamp < CURRENT_DATE() + INTERVAL 1 DAY 
                                    THEN 1 ELSE 0 
                                END) AS todayCount
                            FROM validation_table
                            WHERE teststatus = 'completed' ";*/
                var query = @" SELECT 
                            SUM(CASE 
                                WHEN StartTime >= DATE_SUB(CURRENT_DATE(), INTERVAL 30 DAY)
                                AND StartTime < CURRENT_DATE() + INTERVAL 1 DAY 
                                THEN 1 ELSE 0 
                            END) AS monthCount,
                             SUM(CASE WHEN DATE(StartTime) = CURRENT_DATE() THEN 1 ELSE 0 END) AS todayCount
                            FROM cgstagingdata
                            WHERE reporturl IS NOT NULL 
                                    AND reporturl != '' ";

                if (_roleName != "CG Superadmin")
                {
                    query2 = " AND organization = '" + _orgName + "'";
                }
                query = query + query2;
                using (var command = new MySqlCommand(query, connection))
                {
                    // Parameterize the query to prevent SQL injection
                    // command.Parameters.AddWithValue("@orgName", _orgName);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            monthCount = Convert.ToInt32(reader["monthCount"] == DBNull.Value ? 0 : reader["monthCount"]);
                            todayCount = Convert.ToInt32(reader["todayCount"] == DBNull.Value ? 0 : reader["todayCount"]);
                        }
                    }
                }
            }
            cntTest[0] = monthCount;
            cntTest[1] = todayCount;
            return cntTest;
        }
        catch
        { }
        return cntTest;
    }

    public int[] GetNewUserCounts()
    {
        int[] cntTest = new int[2];
        try
        {
            int monthCount = 0;
            int todayCount = 0;

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var query2 = string.Empty;
                /*var query = @"
                SELECT 
                    SUM(CASE WHEN MONTH(timestamp) = MONTH(CURRENT_DATE()) AND YEAR(timestamp) = YEAR(CURRENT_DATE()) THEN 1 ELSE 0 END) AS monthCount,
                    SUM(CASE WHEN DATE(timestamp) = CURRENT_DATE() THEN 1 ELSE 0 END) AS todayCount
                FROM validation_table
                WHERE register_page = 1 ";*/

                var query = @"SELECT 
                            SUM(CASE 
		                        WHEN timestamp >= DATE_SUB(CURRENT_DATE(), INTERVAL 30 DAY)
		                        AND timestamp < CURRENT_DATE() + INTERVAL 1 DAY 
		                        THEN 1 ELSE 0 
	                        END) AS monthCount,
	                         SUM(CASE WHEN DATE(timestamp) = CURRENT_DATE() THEN 1 ELSE 0 END) AS todayCount
                        FROM validation_table
                        WHERE register_page = 1
                        ";
                if (_roleName != "CG Superadmin")
                {
                    query2 = " AND organisation = '" + _orgName + "'";
                }
                query = query + query2;
                using (var command = new MySqlCommand(query, connection))
                {
                    // Parameterize the query to prevent SQL injection
                    //command.Parameters.AddWithValue("@orgName", _orgName);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            monthCount = Convert.ToInt32(reader["monthCount"] == DBNull.Value ? 0 : reader["monthCount"]);
                            todayCount = Convert.ToInt32(reader["todayCount"] == DBNull.Value ? 0 : reader["todayCount"]);
                        }
                    }
                }
            }
            cntTest[0] = monthCount;
            cntTest[1] = todayCount;
            return cntTest;
        }
        catch
        { }
        return cntTest;
    }

    public int[] GetRepeatLoginCounts()
    {
        int[] cntTest = new int[2];
        try
        {
            int monthCount = 0;
            int todayCount = 0;
            List<int> monthCountIsOne = new List<int>();
            List<int> monthCountMoreThanOne = new List<int>();

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                var spname = "sp_GetTestAttemptsCounts";

                using (var command = new MySqlCommand(spname, connection))
                {
                    // Specify that the command is a stored procedure
                    command.CommandType = CommandType.StoredProcedure;

                    // Add input parameters (if any)
                    command.Parameters.AddWithValue("@orgname", _orgName);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int monthCount1 = Convert.ToInt32(reader["monthCount"]);
                            int todayCount1 = Convert.ToInt32(reader["todayCount"]);

                            // Append the values to the respective lists
                            if (monthCount > 1)
                            {
                                monthCountIsOne.Add(monthCount1);
                            }
                            else if (monthCount > 1)
                            {
                                monthCountMoreThanOne.Add(todayCount1);
                            }
                        }
                    }
                }
            }
            cntTest[0] = monthCountIsOne.Sum(); //month count
            cntTest[1] = monthCountMoreThanOne.Sum();//today count
            return cntTest;
        }
        catch
        { return cntTest; }
    }
}