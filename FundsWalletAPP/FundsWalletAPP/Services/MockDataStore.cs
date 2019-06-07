using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FundsWalletAPP.Models;
using Newtonsoft.Json;

namespace FundsWalletAPP.Services
{
    public class MockDataStore : IDataStore<Item>
    {
        public const string URL = "http://13df6a60.ngrok.io/api";

        List<Item> items;
        HttpClient client;

        public MockDataStore()
        {
            
            items = new List<Item>();
            client = new HttpClient();
      
        }

        public async Task<bool> AddItemAsync(Item item)
        {
            var uri = new Uri(string.Format(URL + "/Stocks", string.Empty));
            var json = JsonConvert.SerializeObject(new Item { Name = "xxxx", Date="11", Price="11", Id="0", Quantity="1"});
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var result = await client.PostAsync(uri, content);
            if (result.IsSuccessStatusCode)
            {
                items.Add(item);
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);


        }

        public async Task<bool> UpdateItemAsync(Item item)
        {
            var oldItem = items.Where((Item arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(oldItem);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var oldItem = items.Where((Item arg) => arg.Id == id).FirstOrDefault();
            items.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<Item> GetItemAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Item>> GetItemsAsync(bool forceRefresh = false)
        {
            var uri = new Uri(string.Format(URL + "/Stocks", string.Empty));
            var response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                List<Item> list = JsonConvert.DeserializeObject<List<Item>>(content);
                items = list;
            }
            return await Task.FromResult(items);
        }
    }
}