using Investimentos.Data;
using Investimentos.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using Investimentos.Services.Exceptions;

namespace Investimentos.Services
{
    public class CarteiraService
    {
        private readonly InvestimentosContext _context;

        public CarteiraService(InvestimentosContext context)
        {
            _context = context;
        }

        // listar todos
        public async Task<List<Carteira>> BuscarTodos()
        {
            return await _context.Carteira
                .OrderBy(x => x.PapelId)
                .ToListAsync();
        }

        // buscar por id
        public async Task<Carteira> BuscarId(int id)
        {
            return await _context.Carteira.Include(carteira => carteira.Papel).FirstOrDefaultAsync(carteira => carteira.Id == id);
        }

        // inserir
        public async Task Inserir(Carteira carteira)
        {
            try
            {
                _context.Add(carteira);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        // editar
        public async Task Editar(Carteira carteira)
        {
            var obj = await _context.Carteira.AnyAsync(x => x.Id == carteira.Id);
            if (!obj)
            {
                throw new NotFoundException("Não encontrado!");
            }
            try
            {
                _context.Update(carteira);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new DbConcurrencyException(e.Message);
            }
        }

        // excluir
        public async Task Remover(int id)
        {
            try
            {
                var carteira = await _context.Carteira.FindAsync(id);
                _context.Carteira.Remove(carteira);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                throw new IntegrityException(e.Message);
            }
        }
    }
}