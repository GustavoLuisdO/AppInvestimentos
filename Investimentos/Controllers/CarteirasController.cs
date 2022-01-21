using Investimentos.Models;
using Investimentos.Services;
using Investimentos.Services.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Investimentos.Controllers
{
    public class CarteirasController : Controller
    {
        private readonly CarteiraService _carteiraService;
        private readonly PapelService _papelService;

        public CarteirasController(CarteiraService carteiraService, PapelService papelService)
        {
            _carteiraService = carteiraService;
            _papelService = papelService;
        }

        // Carteira GET
        public async Task<IActionResult> Index()
        {
            var list = await _carteiraService.BuscarTodos();
            return View(list);
        }

        // Detalhes GET
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id não fonecido!" });
            }

            var obj = await _carteiraService.BuscarId(id.Value);
            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Operação não encontrada!" });
            }
            return View(obj);
        }

        // Criar GET
        public async Task<IActionResult> Create()
        {
            var papeis = await _papelService.BuscarTodos();
            ViewBag.Papeis = papeis;
            return View();
        }

        // Criar POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Carteira carteira)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var papeis = await _papelService.BuscarTodos();
                    ViewBag.Papeis = papeis;
                }
                await _carteiraService.Inserir(carteira);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
        }

        // editar GET
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id não fornecido!" });
            }

            var obj = await _carteiraService.BuscarId(id.Value);
            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Operação não encontrada!" });
            }

            List<Papel> papeis = await _papelService.BuscarTodos();
            ViewBag.Papeis = papeis;

            return View(obj);
        }

        // editar POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Carteira carteira)
        {
            if (id != carteira.Id)
            {
                return RedirectToAction(nameof(Error), new { message = "Id não corresponde!" });
            }

            try
            {
                if (ModelState.IsValid)
                {
                    var papeis = await _papelService.BuscarTodos();
                    ViewBag.Papeis = papeis;
                }
                await _carteiraService.Editar(carteira);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
        }

        // excluir GET
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id não fornecido!" });
            }

            var obj = await _carteiraService.BuscarId(id.Value);
            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Operação não encontrada!" });
            }
            return View(obj);
        }

        // excluir POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _carteiraService.Remover(id);
                return RedirectToAction(nameof(Index));
            }
            catch (IntegrityException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
        }
        
        // Error
        public IActionResult Error(string message)
        {
            var viewModel = new ErrorViewModel
            {
                Message = message,
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };
            return View(viewModel);
        }
    }
}