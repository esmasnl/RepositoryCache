using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Cache.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        #region constants

        const string cacheKey = "catalogKey";

        #endregion

        #region fields

        private readonly IMemoryCache _memCache;

        #endregion

        #region ctor


        public CatalogController(IMemoryCache memCache)
        {
            _memCache = memCache;
        }

        #endregion

        #region methods

        [HttpGet]
        public ActionResult<IEnumerable<Catalog>> Get()
        {
            //Catalog listemize 2 adet kategori set ediyoruz
            List<Catalog> catList = new List<Catalog> { new Catalog { Name = "Diş Macunu", Published = true }, new Catalog { Name = "Parfüm", Published = true } };

            //Burada değerin belirtilen key ile cache'de kontrolünü yapıyoruz
            if (!_memCache.TryGetValue(cacheKey, out catList))
            {
                //Burada cache için belirli ayarlamaları yapıyoruz.Cache süresi,önem derecesi gibi
                var cacheExpOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddMinutes(30),
                    Priority = CacheItemPriority.Normal
                };
                //Bu satırda belirlediğimiz key'e göre ve ayarladığımız cache özelliklerine göre kategorilerimizi in-memory olarak cache'liyoruz.
                _memCache.Set(cacheKey, catList, cacheExpOptions);
            }
            return catList;
        }

        [HttpGet]
        public ActionResult DeleteCache()
        {
            //Remove ile verilen key'e göre bulunan veriyi siliyoruz.
            _memCache.Remove(cacheKey);
            return View();
        }

        #endregion
    }
}