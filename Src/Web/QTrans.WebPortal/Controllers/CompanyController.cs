using AutoMapper;
using QTrans.Models;
using QTrans.Repositories;
using QTrans.WebPortal.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QTrans.WebPortal.Controllers
{
    public class CompanyController : BaseController
    {
        #region =============== Company details================
       
        // GET: User/Details/
        public ActionResult Details()
        {
            var message = string.Empty;
            CompanyRepository repository = new CompanyRepository(this.UserId);
            //Perform the conversion and fetch the destination view model
            var comp = repository.GetCompanyDetailByUserId(this.UserId, out message);
            if (comp != null)
            {
                var Company = Mapper.Map<QTrans.WebPortal.Models.Company>(comp);
                return View(Company);
            }

            return RedirectToAction("Create");
        }

        [AllowAnonymous]
        // GET: User/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Create(Company company)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var message = string.Empty;
                    CompanyRepository repository = new CompanyRepository(this.UserId);
                    //Perform the conversion and fetch the destination view model
                    var comp = Mapper.Map<QTrans.Models.Company>(company);
                    comp.UserId = this.UserId;
                    comp = repository.CompanyRegistration(comp, out message);
                    if (comp != null)
                    {
                        return RedirectToAction("Details/" + comp.companyid.ToString());
                    }
                }
            }
            catch (Exception exp)
            {
                ////TODO: log the error.
            }

            return View();
        }

        // GET: User/Edit/5
        public ActionResult Edit(int id)
        {
            var message = string.Empty;
            CompanyRepository repository = new CompanyRepository(this.UserId);
            //Perform the conversion and fetch the destination view model
            var company = repository.GetCompanyDetailById(id, out message);
            var comp = Mapper.Map<QTrans.WebPortal.Models.Company>(company);
            return View(comp);
        }

        // POST: User/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Company company)
        {
            try
            {
                // TODO: Add update logic here
                if (ModelState.IsValid)
                {
                    company.companyid = id;
                    var message = string.Empty;
                    CompanyRepository repository = new CompanyRepository(this.UserId);
                    //Perform the conversion and fetch the destination view model
                    var comp = Mapper.Map<QTrans.Models.Company>(company);
                    comp.UserId = this.UserId;
                    comp = repository.CompanyRegistration(comp, out message);
                    if (comp != null)
                    {
                        return RedirectToAction("Details/" + id.ToString());
                    }
                }
                /// return RedirectToAction("Details/" + id.ToString());
            }
            catch
            {
                ////TODO: log the error.
            }

            return View();
        }
        #endregion
    }
}