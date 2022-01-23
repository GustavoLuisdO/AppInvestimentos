using Investimentos.Data;
using Investimentos.Models;
using Investimentos.Services.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Investimentos.Services
{
    public class PapelService
    {
        private readonly InvestimentosContext _context;

        public PapelService(InvestimentosContext context)
        {
            _context = context;
        }

        // listar todos
        public async Task<List<Papel>> BuscarTodos()
        {
            return await _context.Papel
                .OrderBy(x => x.Nome)
                .OrderBy(x => x.TipoPapelId)
                .ToListAsync();
        }

        // buscar por Id
        public async Task<Papel> BuscarId(int id)
        {
            return await _context.Papel.Include(papel => papel.TipoPapel).FirstOrDefaultAsync(papel => papel.Id == id);
        }

        // inserir
        public async Task Inserir(Papel papel)
        {
            try
            {
                _context.Add(papel);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        // editar
        public async Task Editar(Papel papel)
        {
            var obj = await _context.Papel.AnyAsync(x => x.Id == papel.Id);
            if (!obj)
            {
                throw new NotFoundException("Papel não encontrado!");
            }
            try
            {
                _context.Update(papel);
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
                var papel = await _context.Papel.FindAsync(id);
                _context.Papel.Remove(papel);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new IntegrityException("Nenhum papel que está associado pode ser excluído!");
            }
        }
    }
}