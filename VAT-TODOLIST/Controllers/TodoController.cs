using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Net.Mail;
using System.Threading.Tasks;
using VAT_TODOLIST.ApplicationDBContext;
using VAT_TODOLIST.Interface;
using VAT_TODOLIST.Models;
using VAT_TODOLIST.Services;
using System.Text;
using VAT_TODOLIST.Utility;
using Microsoft.EntityFrameworkCore;

namespace VAT_TODOLIST.Controllers
{
    public class TodoController : Controller
    {
        private readonly AppUserDb _Dbservice;
        private readonly ILogger<TodoController> _logger;
        private readonly IEmailService _emailservice;
        private readonly Aes _aes;
        

        public TodoController(AppUserDb Dbservice, ILogger<TodoController> logger,IEmailService emailservice)
        {
            _Dbservice = Dbservice;
            _logger = logger;
            _emailservice = emailservice;
            _aes = Aes.Create();
            //_aes.Key = Encoding.UTF8.GetBytes("Your_Encryption_Key");
            _aes.IV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        }        

        public IActionResult Index()
        {
            _logger.LogInformation("Index Page");
            
            try
            {

                IEnumerable<VATTodoModel> objList = _Dbservice.VatTodoDB;
                return View(objList);
                
            }
            catch (Exception ex)
            {                
                _logger.LogError(ex, "This Exception From Index Page");
                throw new Exception();
            }
            
            
        }

        //get
        public IActionResult Create()
        {
            return View();
        }

        //post create
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VATTodoModel obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Check if email already exists in the database
                    var existingEmail = _Dbservice.VatTodoDB.FirstOrDefault(x => x.EmailAddress == obj.EmailAddress);
                    if (existingEmail != null)
                    {
                        TempData["ErrorMessage"] = $"Hey! An email already exists with the same email address: {obj.EmailAddress}";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        _Dbservice.VatTodoDB.Add(obj);
                        _Dbservice.SaveChanges();
                        var emailMsg = new EmailMessageContent(new string[] { obj.EmailAddress }, "NEW TODO ALERT FROM VATINTERNS", $"Dear <b> {obj.Name} </b><br> We are pleased to inform you that you have a new Todo request <br><br> Title: {obj.TaskName} <br> Dated: {obj.TaskDate} <br> Priority: {obj.Priority} <br><br> Best Regards");
                        await _emailservice.SendEmailAsync(emailMsg);
                        TempData["SuccessMessage"] = $"Hey! Todo successfully created! and mail sent to {obj.Name}";
                        return RedirectToAction("Index");
                    }
                }
                return View(obj);
            }
            catch (Exception)
            {
                throw;
            }
        }      


        //get Update
        [HttpGet]
        public IActionResult Update(string Id)
        {
            string decrypt = Encryption.decryptId(Id);
            try
            {
                if (Id == null || Id == "")
                {
                    return NotFound();
                }
                Id = decrypt;
                int id = Convert.ToInt32(Id); ;
                var obj = _Dbservice.VatTodoDB.FirstOrDefault(x => x.Id == id);
                if (obj == null)
                {
                    return NotFound();
                }
                return View(obj);
            }
            catch (Exception)
            {

                throw;
            }         
           
        }

        //Post Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdatePost(VATTodoModel obj, string id)
        {
            try
            {
                string decrypt = Encryption.decryptId(id);
                if (id == null || id == "")
                {
                    return NotFound();
                }
                id = decrypt;
                int id2 = Convert.ToInt32(id);
                var obj2 = _Dbservice.VatTodoDB.FirstOrDefault(x => x.Id == id2);
                if (obj2 != null)
                {
                    obj2.Id = id2;
                    obj2.TaskName = obj.TaskName;
                    obj2.EmailAddress = obj.EmailAddress;
                    obj2.Name = obj.Name;
                    obj2.Priority = obj.Priority;
                   

                    _Dbservice.VatTodoDB.Update(obj2);
                    _Dbservice.SaveChanges();
                }          
                
                var emailMsg = new EmailMessageContent(new string[] { obj.EmailAddress }, "UPDATED TODO ALERT ", $"Dear <b> {obj.Name} </b><br> We are pleased to inform you that your Todo request has been updated <br><br> Title: {obj.TaskName} <br> Dated: {obj.TaskDate} <br> Priority: {obj.Priority} <br><br> Best Regards");
                _emailservice.SendEmailAsync(emailMsg);
                TempData["SuccessMessage"] = "Hey! Todo successfully Updated";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while updating the Todo. Please try again.");
                return View(obj);
            }
        }

        //get delete
        public IActionResult Delete(string Id)
        {
            string decrypt = Encryption.decryptId(Id);

            var data = new VATTodoViewModel();

            try
            {
                if (Id == null || Id == "")
                {
                    return NotFound();
                }
                
                int id = Convert.ToInt32(decrypt); ;
                var obj = _Dbservice.VatTodoDB.FirstOrDefault(x => x.Id == id);
                if (obj == null)
                {
                    return NotFound();
                }
             
                data.HiddenId= Id;
              
                data.EmailAddress = obj.EmailAddress;
                data.Priority=obj.Priority;
                data.Name = obj.Name;
                data.TaskName = obj.TaskName;
                data.TaskDate = obj.TaskDate;
                

                return View(data);
            }
            catch (Exception)
            {

                throw;
            }
            //try
            //{
            //    if (id == null)
            //    {
            //        return NotFound();
            //    }
            //    var obj = _Dbservice.VatTodoDB.Find(id);
            //    if (obj == null)
            //    {
            //        return NotFound();
            //    }
            //    return View(obj);
            //}
            //catch (Exception)
            //{
            //    throw;
            //}

        }

        //post Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(VATTodoViewModel obj)
        {
            string Id = "";
            try
            {
                string decrypt = Encryption.decryptId(obj.HiddenId);
                if (obj.HiddenId == null || obj.HiddenId == "")
                {
                    TempData["SuccessMessage"] = "Hey! Todo successfully Updated";
                    return RedirectToAction("Index");
                }
                Id = decrypt;
                int id2 = Convert.ToInt32(decrypt);
                var obj2 = _Dbservice.VatTodoDB.FirstOrDefault(x => x.Id == id2);
                if (obj2 != null)
                {
                    obj2.Id = id2;
                    _Dbservice.VatTodoDB.Remove(obj2);
                    _Dbservice.SaveChanges();
                    
                }

                TempData["SuccessMessage"] = "Hey! Todo successfully Deleted";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {

                throw;
            }



            //try
            //{
            //    var obj = _Dbservice.VatTodoDB.Find(id);
            //    if (obj == null)
            //    {
            //        return NotFound();
            //    }

            //    _Dbservice.VatTodoDB.Remove(obj);
            //    _Dbservice.SaveChanges();
            //    TempData["SuccessMessage"] = "Hey! Todo successfully Deleted";
            //    return RedirectToAction("Index");

            //}
            //catch (Exception)
            //{
            //    throw;
            //}            
        }
        
    }
}
