using Donation_Website.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System;

namespace Donation_Website.Pages
{
    public class TaskAssignModel : PageModel
    {
        private readonly DBConnection _db;

        public TaskAssignModel()
        {
            _db = new DBConnection();
        }

        [BindProperty]
        public List<VolunteerViewModel> Volunteers { get; set; } = new List<VolunteerViewModel>();

        public void OnGet()
        {
            Volunteers = LoadVerifiedVolunteers();
        }

        public IActionResult OnPostAssign(int volunteerId, [FromForm] Dictionary<int, string> duties)
        {
            if (duties != null && duties.TryGetValue(volunteerId, out var duty) && !string.IsNullOrWhiteSpace(duty))
            {
                using (var cmd = _db.GetQuery("INSERT INTO VolunteerAssignment (VolunteerID, RoleTask, Status, AssignDate) VALUES (@VolunteerID, @RoleTask, @Status, @AssignDate)"))
                {
                    cmd.Parameters.AddWithValue("@VolunteerID", volunteerId);
                    cmd.Parameters.AddWithValue("@RoleTask", duty);
                    cmd.Parameters.AddWithValue("@Status", "Assigned");
                    cmd.Parameters.AddWithValue("@AssignDate", DateTime.Now);

                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    cmd.Connection.Close();
                }

                TempData["SuccessMessage"] = "Task assigned successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Duty description cannot be empty for assignment.";
            }

            return RedirectToPage();
        }

        private List<VolunteerViewModel> LoadVerifiedVolunteers()
        {
            var list = new List<VolunteerViewModel>();

            using (var cmd = _db.GetQuery("SELECT VolunteerID, Name, Email, Phone, Address, Skill, Availability FROM Volunteer WHERE IsActive = 1"))
            {
                cmd.Connection.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new VolunteerViewModel
                        {
                            VolunteerID = reader["VolunteerID"] == DBNull.Value ? 0 : Convert.ToInt32(reader["VolunteerID"]),
                            Name = reader["Name"]?.ToString() ?? "",
                            Email = reader["Email"]?.ToString() ?? "",
                            Phone = reader["Phone"]?.ToString() ?? "",
                            Address = reader["Address"]?.ToString() ?? "",
                            Skill = reader["Skill"]?.ToString() ?? "",
                            Availability = reader["Availability"]?.ToString() ?? ""
                        });
                    }
                }
                cmd.Connection.Close();
            }

            return list;
        }
    }
}