﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Servicios.api.Libreria.Core.Entities;
using Servicios.api.Libreria.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicios.api.Libreria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibreriaAutorController : ControllerBase
    {
        private readonly IMongoRepository<AutorEntity> _autorGenericoRepository;

        public LibreriaAutorController(IMongoRepository<AutorEntity> autorGenericoRepository)
        {
            _autorGenericoRepository = autorGenericoRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AutorEntity>>> Get()
        {
            return Ok(await _autorGenericoRepository.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AutorEntity>> GetById(string id)
        {
            var autor = await _autorGenericoRepository.GetById(id);

            return Ok(autor);
        }

        [HttpPost]
        public async Task Post(AutorEntity autor)
        {
            await _autorGenericoRepository.InsertDocument(autor);
        }

        [HttpPut("{id}")]
        public async Task Put(string id, AutorEntity autor)
        {
            autor.Id = id;
            await _autorGenericoRepository.UpdateDocument(autor);
        }

        [HttpDelete("{id}")]
        public async Task Delete(string id)
        {
            await _autorGenericoRepository.DeleteById(id);
        }

        [HttpPost("pagination")]
        public async Task<ActionResult<PaginationEntity<AutorEntity>>> PostPagination(PaginationEntity<AutorEntity> pagination)
        {
            var resultados = await _autorGenericoRepository.PaginationBy(filter => filter.Nombre == pagination.Filter, pagination);

            return Ok(resultados);
        }

        [HttpPost("paginationByFilter")]
        public async Task<ActionResult<PaginationEntity<AutorEntity>>> PostPaginationByFilter(PaginationEntity<AutorEntity> pagination)
        {
            var resultados = await _autorGenericoRepository.PaginationByFilter(pagination);

            return Ok(resultados);
        }
    }
}
