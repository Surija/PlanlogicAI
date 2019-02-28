using AutoMapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using PlanlogicAI.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;

namespace PlanlogicAI.Controllers
{
    public class APIUpdate
    {
        public readonly StrategyOptimizerPrototypeContext context;
        public readonly IMapper mapper;

        public APIUpdate(StrategyOptimizerPrototypeContext context, IMapper mapper)
            {
            this.context = context;
            this.mapper = mapper;
        }

        public partial class APIDetailsModel
        {
            [JsonProperty("api", NullValueHandling = NullValueHandling.Ignore)]
            public Api Api { get; set; }
        }

        public partial class Api
        {

            //[JsonProperty("@_id")]
            //public Id Id { get; set; }

            [JsonProperty("FSCBI-MStarID")]
            public string MId { get; set; }

            [JsonProperty("FSCBI-APIR", NullValueHandling = NullValueHandling.Ignore)]
            public string Apircode { get; set; }

            [JsonProperty("FSCBI-FundName")]
            public string FundName { get; set; }

            [JsonProperty("PF-TransactionFee", NullValueHandling = NullValueHandling.Ignore)]
            public decimal BuySpread { get; set; }

            [JsonProperty("ARF-IndirectCostRatio", NullValueHandling = NullValueHandling.Ignore)]
            public decimal Icr { get; set; }

            [JsonProperty("ASAA-AUSSurveyedAssetAllocation", NullValueHandling = NullValueHandling.Ignore)]
            public AssetAllocationDetails? AssetAllocation { get; set; }
        }

        public partial class AsaaAusSurveyedAssetAllocationElement
        {
            [JsonProperty("Type")]
            public TypeEnum Type { get; set; }

            [JsonProperty("Value")]
            public decimal Value { get; set; }
        }

        public enum TypeEnum { DomesticEquity, InternationalEquity, DomesticProperty, InternationalProperty, GrowthAlternatives, DefensiveAlternatives, DomesticFixedInterest, InternationalFixedInterest, DomesticCash, InternationalCash, OtherGrowth };

        public partial struct AssetAllocationDetails
        {
            public AsaaAusSurveyedAssetAllocationElement AssetAllocationElement;
            public List<AsaaAusSurveyedAssetAllocationElement> AssetAllocationElementArray;

            public static implicit operator AssetAllocationDetails(AsaaAusSurveyedAssetAllocationElement AsaaAusSurveyedAssetAllocationElement) => new AssetAllocationDetails { AssetAllocationElement = AsaaAusSurveyedAssetAllocationElement };
            public static implicit operator AssetAllocationDetails(List<AsaaAusSurveyedAssetAllocationElement> AsaaAusSurveyedAssetAllocationElementArray) => new AssetAllocationDetails { AssetAllocationElementArray = AsaaAusSurveyedAssetAllocationElementArray };
        }

        internal static class Converter
        {
            public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
            {
                MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
                DateParseHandling = DateParseHandling.None,
                Converters =
            {

                AsaaAusSurveyedAssetAllocationUnionConverter.Singleton,
                TypeEnumConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
            };
        }

        internal class AsaaAusSurveyedAssetAllocationUnionConverter : JsonConverter
        {
            public override bool CanConvert(Type t) => t == typeof(AssetAllocationDetails) || t == typeof(AssetAllocationDetails?);

            public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
            {
                switch (reader.TokenType)
                {
                    case JsonToken.StartObject:
                        var objectValue = serializer.Deserialize<AsaaAusSurveyedAssetAllocationElement>(reader);
                        return new AssetAllocationDetails { AssetAllocationElement = objectValue };
                    case JsonToken.StartArray:
                        var arrayValue = serializer.Deserialize<List<AsaaAusSurveyedAssetAllocationElement>>(reader);
                        return new AssetAllocationDetails { AssetAllocationElementArray = arrayValue };
                }
                throw new Exception("Cannot unmarshal type AsaaAusSurveyedAssetAllocationUnion");
            }

            public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
            {
                var value = (AssetAllocationDetails)untypedValue;
                if (value.AssetAllocationElementArray != null)
                {
                    serializer.Serialize(writer, value.AssetAllocationElementArray);
                    return;
                }
                if (value.AssetAllocationElement != null)
                {
                    serializer.Serialize(writer, value.AssetAllocationElement);
                    return;
                }
                throw new Exception("Cannot marshal type AsaaAusSurveyedAssetAllocationUnion");
            }

            public static readonly AsaaAusSurveyedAssetAllocationUnionConverter Singleton = new AsaaAusSurveyedAssetAllocationUnionConverter();
        }

        internal class TypeEnumConverter : JsonConverter
        {
            public override bool CanConvert(Type t) => t == typeof(TypeEnum) || t == typeof(TypeEnum?);

            public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
            {
                if (reader.TokenType == JsonToken.Null) return null;
                var value = serializer.Deserialize<string>(reader);
                switch (value)
                {
                    case "Australia Alternative Assets":
                        return TypeEnum.GrowthAlternatives;
                    case "Australian Bonds - Unspecified":
                        return TypeEnum.DomesticFixedInterest;
                    case "Australian Cash":
                        return TypeEnum.DomesticCash;
                    case "Australian Corporate Bonds":
                        return TypeEnum.DomesticFixedInterest;
                    case "Australian Government Bonds":
                        return TypeEnum.DomesticFixedInterest;
                    case "Australian Listed Shares - Consumer Discretionary":
                        return TypeEnum.DomesticEquity;
                    case "Australian Listed Shares - Consumer Staples":
                        return TypeEnum.DomesticEquity;
                    case "Australian Listed Shares - Energy":
                        return TypeEnum.DomesticEquity;
                    case "Australian Listed Shares - Financials ex Property Trusts":
                        return TypeEnum.DomesticEquity;
                    case "Australian Listed Shares - Health Care":
                        return TypeEnum.DomesticEquity;
                    case "Australian Listed Shares - Industrials":
                        return TypeEnum.DomesticEquity;
                    case "Australian Listed Shares - Information Technology":
                        return TypeEnum.DomesticEquity;
                    case "Australian Listed Shares - Materials":
                        return TypeEnum.DomesticEquity;
                    case "Australian Listed Shares - Property Trusts":
                        return TypeEnum.DomesticProperty;
                    case "Australian Listed Shares - Telecommunication Services":
                        return TypeEnum.DomesticEquity;
                    case "Australian Listed Shares - Unspecified":
                        return TypeEnum.DomesticEquity;
                    case "Australian Listed Shares - Utilities":
                        return TypeEnum.DomesticEquity;
                    case "Australian Unlisted Property":
                        return TypeEnum.DomesticProperty;
                    case "Australian Unlisted Shares":
                        return TypeEnum.DomesticEquity;
                    case "Infrastructure":
                        return TypeEnum.OtherGrowth;
                    case "International Alternative Assets":
                        return TypeEnum.GrowthAlternatives;
                    case "International Bonds - Unspecified":
                        return TypeEnum.InternationalFixedInterest;
                    case "International Cash":
                        return TypeEnum.InternationalCash;
                    case "International Listed Property":
                        return TypeEnum.InternationalProperty;
                    case "International Listed Shares - Africa":
                        return TypeEnum.InternationalEquity;
                    case "International Listed Shares - Austria":
                        return TypeEnum.InternationalEquity;
                    case "International Listed Shares - Belgium":
                        return TypeEnum.InternationalEquity;
                    case "International Listed Shares - Canada":
                        return TypeEnum.InternationalEquity;
                    case "International Listed Shares - Denmark":
                        return TypeEnum.InternationalEquity;
                    case "International Listed Shares - Finland":
                        return TypeEnum.InternationalEquity;
                    case "International Listed Shares - France":
                        return TypeEnum.InternationalEquity;
                    case "International Listed Shares - Germany":
                        return TypeEnum.InternationalEquity;
                    case "International Listed Shares - Hong Kong":
                        return TypeEnum.InternationalEquity;
                    case "International Listed Shares - Ireland":
                        return TypeEnum.InternationalEquity;
                    case "International Listed Shares - Italy":
                        return TypeEnum.InternationalEquity;
                    case "International Listed Shares - Japan":
                        return TypeEnum.InternationalEquity;
                    case "International Listed Shares - Latin America":
                        return TypeEnum.InternationalEquity;
                    case "International Listed Shares - Malaysia":
                        return TypeEnum.InternationalEquity;
                    case "International Listed Shares - Middle East":
                        return TypeEnum.InternationalEquity;
                    case "International Listed Shares - Netherlands":
                        return TypeEnum.InternationalEquity;
                    case "International Listed Shares - Norway":
                        return TypeEnum.InternationalEquity;
                    case "International Listed Shares - Other Asia":
                        return TypeEnum.InternationalEquity;
                    case "International Listed Shares - Other Europe":
                        return TypeEnum.InternationalEquity;
                    case "International Listed Shares - Portugal":
                        return TypeEnum.InternationalEquity;
                    case "International Listed Shares - Singapore":
                        return TypeEnum.InternationalEquity;
                    case "International Listed Shares - Spain":
                        return TypeEnum.InternationalEquity;
                    case "International Listed Shares - Sweden":
                        return TypeEnum.InternationalEquity;
                    case "International Listed Shares - Switzerland":
                        return TypeEnum.InternationalEquity;
                    case "International Listed Shares - USA":
                        return TypeEnum.InternationalEquity;
                    case "International Listed Shares - United Kingdom":
                        return TypeEnum.InternationalEquity;
                    case "International Listed Shares - Unspecified":
                        return TypeEnum.InternationalEquity;
                    case "International Unlisted Property":
                        return TypeEnum.InternationalProperty;
                    case "International Unlisted Shares":
                        return TypeEnum.InternationalEquity;
                    case "Mortgages":
                        return TypeEnum.DomesticFixedInterest;
                    case "New Zealand Bonds - Unspecified":
                        return TypeEnum.InternationalFixedInterest;
                    case "New Zealand Cash":
                        return TypeEnum.InternationalCash;
                    case "New Zealand Listed Shares - Unspecified":
                        return TypeEnum.InternationalEquity;
                    case "Other / Unclassified":
                        return TypeEnum.OtherGrowth;
                }
                throw new Exception("Cannot unmarshal type TypeEnum");
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                throw new NotImplementedException();
            }



            public static readonly TypeEnumConverter Singleton = new TypeEnumConverter();
        }

        public  void SendAsync()
            {
            try
            {

                string url = "";
                url = @"https://api.morningstar.com/v2/service/mf/ugkyhaskr79mdqge/universeid/y4266l541ybiknfe?accesscode=sklfgghyv7g0fl51klfheqdue7ol0te1";


                using (var response = ApiHelper.ApiClient.GetAsync(url).Result)
                {
                    
                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = response.Content;
                       // return responseContent.ReadAsStringAsync().Result;

                        var content =  response.Content.ReadAsStringAsync().Result;
                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(content);
                        string jsonText = JsonConvert.SerializeXmlNode(doc);

                        var res = JObject.Parse(jsonText);
                        var data1 = (res["response"] as JObject);
                        var apiDet = (data1["data"] as JArray).ToString();
                        var result = JsonConvert.DeserializeObject<List<APIDetailsModel>>(apiDet, Converter.Settings);
                        List<InvestmentFundViewModel> fundArray = new List<InvestmentFundViewModel>();

                        foreach (APIDetailsModel api in result)
                        {
                            if (api.Api.Apircode != null && api.Api.Apircode != "")
                            {
                                InvestmentFundViewModel _fund = new InvestmentFundViewModel();

                                _fund.Apircode = api.Api.Apircode;
                                _fund.MId = api.Api.MId;
                                _fund.FundName = api.Api.FundName;
                                _fund.BuySpread = api.Api.BuySpread;
                                _fund.Icr = api.Api.Icr;

                                if (api.Api.AssetAllocation.HasValue && api.Api.AssetAllocation != null)
                                {
                                    if (api.Api.AssetAllocation.Value.AssetAllocationElementArray != null)
                                    {
                                        _fund.DomesticEquity = api.Api.AssetAllocation.Value.AssetAllocationElementArray.Where(c => c.Type.ToString() == "DomesticEquity").Any() ? api.Api.AssetAllocation.Value.AssetAllocationElementArray.Where(c => c.Type.ToString() == "DomesticEquity").Sum(c => c.Value) : 0;
                                        _fund.InternationalEquity = api.Api.AssetAllocation.Value.AssetAllocationElementArray.Where(c => c.Type.ToString() == "InternationalEquity").Any() ? api.Api.AssetAllocation.Value.AssetAllocationElementArray.Where(c => c.Type.ToString() == "InternationalEquity").Sum(c => c.Value) : 0;
                                        _fund.DomesticProperty = api.Api.AssetAllocation.Value.AssetAllocationElementArray.Where(c => c.Type.ToString() == "DomesticProperty").Any() ? api.Api.AssetAllocation.Value.AssetAllocationElementArray.Where(c => c.Type.ToString() == "DomesticProperty").Sum(c => c.Value) : 0;
                                        _fund.InternationalProperty = api.Api.AssetAllocation.Value.AssetAllocationElementArray.Where(c => c.Type.ToString() == "InternationalProperty").Any() ? api.Api.AssetAllocation.Value.AssetAllocationElementArray.Where(c => c.Type.ToString() == "InternationalProperty").Sum(c => c.Value) : 0;
                                        _fund.GrowthAlternatives = api.Api.AssetAllocation.Value.AssetAllocationElementArray.Where(c => c.Type.ToString() == "GrowthAlternatives").Any() ? api.Api.AssetAllocation.Value.AssetAllocationElementArray.Where(c => c.Type.ToString() == "GrowthAlternatives").Sum(c => c.Value) : 0;
                                        _fund.DefensiveAlternatives = api.Api.AssetAllocation.Value.AssetAllocationElementArray.Where(c => c.Type.ToString() == "DefensiveAlternatives").Any() ? api.Api.AssetAllocation.Value.AssetAllocationElementArray.Where(c => c.Type.ToString() == "DefensiveAlternatives").Sum(c => c.Value) : 0;
                                        _fund.DomesticFixedInterest = api.Api.AssetAllocation.Value.AssetAllocationElementArray.Where(c => c.Type.ToString() == "DomesticFixedInterest").Any() ? api.Api.AssetAllocation.Value.AssetAllocationElementArray.Where(c => c.Type.ToString() == "DomesticFixedInterest").Sum(c => c.Value) : 0;
                                        _fund.InternationalFixedInterest = api.Api.AssetAllocation.Value.AssetAllocationElementArray.Where(c => c.Type.ToString() == "InternationalFixedInterest").Any() ? api.Api.AssetAllocation.Value.AssetAllocationElementArray.Where(c => c.Type.ToString() == "InternationalFixedInterest").Sum(c => c.Value) : 0;
                                        _fund.DomesticCash = api.Api.AssetAllocation.Value.AssetAllocationElementArray.Where(c => c.Type.ToString() == "DomesticCash").Any() ? api.Api.AssetAllocation.Value.AssetAllocationElementArray.Where(c => c.Type.ToString() == "DomesticCash").Sum(c => c.Value) : 0;
                                        _fund.InternationalCash = api.Api.AssetAllocation.Value.AssetAllocationElementArray.Where(c => c.Type.ToString() == "InternationalCash").Any() ? api.Api.AssetAllocation.Value.AssetAllocationElementArray.Where(c => c.Type.ToString() == "InternationalCash").Sum(c => c.Value) : 0;
                                        _fund.OtherGrowth = api.Api.AssetAllocation.Value.AssetAllocationElementArray.Where(c => c.Type.ToString() == "OtherGrowth").Any() ? api.Api.AssetAllocation.Value.AssetAllocationElementArray.Where(c => c.Type.ToString() == "OtherGrowth").Sum(c => c.Value) : 0;
                                    }
                                    else if (api.Api.AssetAllocation.Value.AssetAllocationElement != null)
                                    {
                                        _fund.DomesticEquity = api.Api.AssetAllocation.Value.AssetAllocationElement.Type.ToString() == "DomesticEquity" ? api.Api.AssetAllocation.Value.AssetAllocationElement.Value : 0;
                                        _fund.InternationalEquity = api.Api.AssetAllocation.Value.AssetAllocationElement.Type.ToString() == "InternationalEquity" ? api.Api.AssetAllocation.Value.AssetAllocationElement.Value : 0;
                                        _fund.DomesticProperty = api.Api.AssetAllocation.Value.AssetAllocationElement.Type.ToString() == "DomesticProperty" ? api.Api.AssetAllocation.Value.AssetAllocationElement.Value : 0;
                                        _fund.InternationalProperty = api.Api.AssetAllocation.Value.AssetAllocationElement.Type.ToString() == "InternationalProperty" ? api.Api.AssetAllocation.Value.AssetAllocationElement.Value : 0;
                                        _fund.GrowthAlternatives = api.Api.AssetAllocation.Value.AssetAllocationElement.Type.ToString() == "GrowthAlternatives" ? api.Api.AssetAllocation.Value.AssetAllocationElement.Value : 0;
                                        _fund.DefensiveAlternatives = api.Api.AssetAllocation.Value.AssetAllocationElement.Type.ToString() == "DefensiveAlternatives" ? api.Api.AssetAllocation.Value.AssetAllocationElement.Value : 0;
                                        _fund.DomesticFixedInterest = api.Api.AssetAllocation.Value.AssetAllocationElement.Type.ToString() == "DomesticFixedInterest" ? api.Api.AssetAllocation.Value.AssetAllocationElement.Value : 0;
                                        _fund.InternationalFixedInterest = api.Api.AssetAllocation.Value.AssetAllocationElement.Type.ToString() == "InternationalFixedInterest" ? api.Api.AssetAllocation.Value.AssetAllocationElement.Value : 0;
                                        _fund.DomesticCash = api.Api.AssetAllocation.Value.AssetAllocationElement.Type.ToString() == "DomesticCash" ? api.Api.AssetAllocation.Value.AssetAllocationElement.Value : 0;
                                        _fund.InternationalCash = api.Api.AssetAllocation.Value.AssetAllocationElement.Type.ToString() == "InternationalCash" ? api.Api.AssetAllocation.Value.AssetAllocationElement.Value : 0;
                                        _fund.OtherGrowth = api.Api.AssetAllocation.Value.AssetAllocationElement.Type.ToString() == "OtherGrowth" ? api.Api.AssetAllocation.Value.AssetAllocationElement.Value : 0;
                                    }
                                }
                                else
                                {
                                    _fund.DomesticEquity = 0;
                                    _fund.InternationalEquity = 0;
                                    _fund.DomesticProperty = 0;
                                    _fund.InternationalProperty = 0;
                                    _fund.GrowthAlternatives = 0;
                                    _fund.DefensiveAlternatives = 0;
                                    _fund.DomesticFixedInterest = 0;
                                    _fund.InternationalFixedInterest = 0;
                                    _fund.DomesticCash = 0;
                                    _fund.InternationalCash = 0;
                                    _fund.OtherGrowth = 0;

                                }


                                _fund.UpdatedOn = DateTime.Now;
                                fundArray.Add(_fund);
                            }


                        }

                        if (fundArray.Count > 0)
                        {

                            using (var dbContextTransaction = context.Database.BeginTransaction())
                            {
                                try
                                {
                                    foreach (InvestmentFundViewModel _fund in fundArray)
                                    {

                                        var existingFund = this.context.InvestmentFund.Where(b => b.Apircode == _fund.Apircode).FirstOrDefault();
                                        if (existingFund == null)
                                        {
                                            var fund = mapper.Map<InvestmentFund>(_fund);
                                            this.context.InvestmentFund.Add(fund);
                                            this.context.SaveChanges();

                                        }
                                        else
                                        {
                                            this.mapper.Map<InvestmentFundViewModel, InvestmentFund>(_fund, existingFund);

                                            this.context.InvestmentFund.Update(existingFund);
                                            this.context.SaveChanges();

                                        }
                                    }

                                    dbContextTransaction.Commit();
                                }
                                catch (Exception ex)
                                {
                                    throw ex;
                                }
                            }
                        }

                    }
                    else
                    {
                        throw new Exception(response.ReasonPhrase);
                    }
                
                }

                //using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url))
                //{
                //    if (response.IsSuccessStatusCode)
                //    {
                //        var content = await response.Content.ReadAsStringAsync();
                //        XmlDocument doc = new XmlDocument();
                //        doc.LoadXml(content);
                //        string jsonText = JsonConvert.SerializeXmlNode(doc);

                //        var res = JObject.Parse(jsonText);
                //        var data1 = (res["response"] as JObject);
                //        var apiDet = (data1["data"] as JArray).ToString();
                //        var result = JsonConvert.DeserializeObject<List<APIDetailsModel>>(apiDet, Converter.Settings);
                //        List<InvestmentFundViewModel> fundArray = new List<InvestmentFundViewModel>();

                //        foreach (APIDetailsModel api in result)
                //        {
                //            if (api.Api.Apircode != null && api.Api.Apircode != "")
                //            {
                //                InvestmentFundViewModel _fund = new InvestmentFundViewModel();

                //                _fund.Apircode = api.Api.Apircode;
                //                _fund.MId = api.Api.MId;
                //                _fund.FundName = api.Api.FundName;
                //                _fund.BuySpread = api.Api.BuySpread;
                //                _fund.Icr = api.Api.Icr;

                //                if (api.Api.AssetAllocation.HasValue && api.Api.AssetAllocation != null)
                //                {
                //                    if (api.Api.AssetAllocation.Value.AssetAllocationElementArray != null)
                //                    {
                //                        _fund.DomesticEquity = api.Api.AssetAllocation.Value.AssetAllocationElementArray.Where(c => c.Type.ToString() == "DomesticEquity").Any() ? api.Api.AssetAllocation.Value.AssetAllocationElementArray.Where(c => c.Type.ToString() == "DomesticEquity").Sum(c => c.Value) : 0;
                //                        _fund.InternationalEquity = api.Api.AssetAllocation.Value.AssetAllocationElementArray.Where(c => c.Type.ToString() == "InternationalEquity").Any() ? api.Api.AssetAllocation.Value.AssetAllocationElementArray.Where(c => c.Type.ToString() == "InternationalEquity").Sum(c => c.Value) : 0;
                //                        _fund.DomesticProperty = api.Api.AssetAllocation.Value.AssetAllocationElementArray.Where(c => c.Type.ToString() == "DomesticProperty").Any() ? api.Api.AssetAllocation.Value.AssetAllocationElementArray.Where(c => c.Type.ToString() == "DomesticProperty").Sum(c => c.Value) : 0;
                //                        _fund.InternationalProperty = api.Api.AssetAllocation.Value.AssetAllocationElementArray.Where(c => c.Type.ToString() == "InternationalProperty").Any() ? api.Api.AssetAllocation.Value.AssetAllocationElementArray.Where(c => c.Type.ToString() == "InternationalProperty").Sum(c => c.Value) : 0;
                //                        _fund.GrowthAlternatives = api.Api.AssetAllocation.Value.AssetAllocationElementArray.Where(c => c.Type.ToString() == "GrowthAlternatives").Any() ? api.Api.AssetAllocation.Value.AssetAllocationElementArray.Where(c => c.Type.ToString() == "GrowthAlternatives").Sum(c => c.Value) : 0;
                //                        _fund.DefensiveAlternatives = api.Api.AssetAllocation.Value.AssetAllocationElementArray.Where(c => c.Type.ToString() == "DefensiveAlternatives").Any() ? api.Api.AssetAllocation.Value.AssetAllocationElementArray.Where(c => c.Type.ToString() == "DefensiveAlternatives").Sum(c => c.Value) : 0;
                //                        _fund.DomesticFixedInterest = api.Api.AssetAllocation.Value.AssetAllocationElementArray.Where(c => c.Type.ToString() == "DomesticFixedInterest").Any() ? api.Api.AssetAllocation.Value.AssetAllocationElementArray.Where(c => c.Type.ToString() == "DomesticFixedInterest").Sum(c => c.Value) : 0;
                //                        _fund.InternationalFixedInterest = api.Api.AssetAllocation.Value.AssetAllocationElementArray.Where(c => c.Type.ToString() == "InternationalFixedInterest").Any() ? api.Api.AssetAllocation.Value.AssetAllocationElementArray.Where(c => c.Type.ToString() == "InternationalFixedInterest").Sum(c => c.Value) : 0;
                //                        _fund.DomesticCash = api.Api.AssetAllocation.Value.AssetAllocationElementArray.Where(c => c.Type.ToString() == "DomesticCash").Any() ? api.Api.AssetAllocation.Value.AssetAllocationElementArray.Where(c => c.Type.ToString() == "DomesticCash").Sum(c => c.Value) : 0;
                //                        _fund.InternationalCash = api.Api.AssetAllocation.Value.AssetAllocationElementArray.Where(c => c.Type.ToString() == "InternationalCash").Any() ? api.Api.AssetAllocation.Value.AssetAllocationElementArray.Where(c => c.Type.ToString() == "InternationalCash").Sum(c => c.Value) : 0;
                //                        _fund.OtherGrowth = api.Api.AssetAllocation.Value.AssetAllocationElementArray.Where(c => c.Type.ToString() == "OtherGrowth").Any() ? api.Api.AssetAllocation.Value.AssetAllocationElementArray.Where(c => c.Type.ToString() == "OtherGrowth").Sum(c => c.Value) : 0;
                //                    }
                //                    else if (api.Api.AssetAllocation.Value.AssetAllocationElement != null)
                //                    {
                //                        _fund.DomesticEquity = api.Api.AssetAllocation.Value.AssetAllocationElement.Type.ToString() == "DomesticEquity" ? api.Api.AssetAllocation.Value.AssetAllocationElement.Value : 0;
                //                        _fund.InternationalEquity = api.Api.AssetAllocation.Value.AssetAllocationElement.Type.ToString() == "InternationalEquity" ? api.Api.AssetAllocation.Value.AssetAllocationElement.Value : 0;
                //                        _fund.DomesticProperty = api.Api.AssetAllocation.Value.AssetAllocationElement.Type.ToString() == "DomesticProperty" ? api.Api.AssetAllocation.Value.AssetAllocationElement.Value : 0;
                //                        _fund.InternationalProperty = api.Api.AssetAllocation.Value.AssetAllocationElement.Type.ToString() == "InternationalProperty" ? api.Api.AssetAllocation.Value.AssetAllocationElement.Value : 0;
                //                        _fund.GrowthAlternatives = api.Api.AssetAllocation.Value.AssetAllocationElement.Type.ToString() == "GrowthAlternatives" ? api.Api.AssetAllocation.Value.AssetAllocationElement.Value : 0;
                //                        _fund.DefensiveAlternatives = api.Api.AssetAllocation.Value.AssetAllocationElement.Type.ToString() == "DefensiveAlternatives" ? api.Api.AssetAllocation.Value.AssetAllocationElement.Value : 0;
                //                        _fund.DomesticFixedInterest = api.Api.AssetAllocation.Value.AssetAllocationElement.Type.ToString() == "DomesticFixedInterest" ? api.Api.AssetAllocation.Value.AssetAllocationElement.Value : 0;
                //                        _fund.InternationalFixedInterest = api.Api.AssetAllocation.Value.AssetAllocationElement.Type.ToString() == "InternationalFixedInterest" ? api.Api.AssetAllocation.Value.AssetAllocationElement.Value : 0;
                //                        _fund.DomesticCash = api.Api.AssetAllocation.Value.AssetAllocationElement.Type.ToString() == "DomesticCash" ? api.Api.AssetAllocation.Value.AssetAllocationElement.Value : 0;
                //                        _fund.InternationalCash = api.Api.AssetAllocation.Value.AssetAllocationElement.Type.ToString() == "InternationalCash" ? api.Api.AssetAllocation.Value.AssetAllocationElement.Value : 0;
                //                        _fund.OtherGrowth = api.Api.AssetAllocation.Value.AssetAllocationElement.Type.ToString() == "OtherGrowth" ? api.Api.AssetAllocation.Value.AssetAllocationElement.Value : 0;
                //                    }
                //                }
                //                else
                //                {
                //                    _fund.DomesticEquity = 0;
                //                    _fund.InternationalEquity = 0;
                //                    _fund.DomesticProperty = 0;
                //                    _fund.InternationalProperty = 0;
                //                    _fund.GrowthAlternatives = 0;
                //                    _fund.DefensiveAlternatives = 0;
                //                    _fund.DomesticFixedInterest = 0;
                //                    _fund.InternationalFixedInterest = 0;
                //                    _fund.DomesticCash = 0;
                //                    _fund.InternationalCash = 0;
                //                    _fund.OtherGrowth = 0;

                //                }


                //                _fund.UpdatedOn = DateTime.Now;
                //                fundArray.Add(_fund);
                //            }


                //        }

                //        if (fundArray.Count > 0)
                //        {

                //            using (var dbContextTransaction = context.Database.BeginTransaction())
                //            {
                //                try
                //                {
                //                    foreach (InvestmentFundViewModel _fund in fundArray)
                //                    {

                //                        var existingFund = this.context.InvestmentFund.Where(b => b.Apircode == _fund.Apircode).FirstOrDefault();
                //                        if (existingFund == null)
                //                        {
                //                            var fund = mapper.Map<InvestmentFund>(_fund);
                //                            this.context.InvestmentFund.Add(fund);
                //                            this.context.SaveChanges();

                //                        }
                //                        else
                //                        {
                //                            this.mapper.Map<InvestmentFundViewModel, InvestmentFund>(_fund, existingFund);

                //                            this.context.InvestmentFund.Update(existingFund);
                //                            this.context.SaveChanges();

                //                        }
                //                    }

                //                    dbContextTransaction.Commit();
                //                }
                //                catch (Exception ex)
                //                {
                //                    throw ex;
                //                }
                //            }
                //        }

                //    }
                //    else
                //    {
                //        throw new Exception(response.ReasonPhrase);
                //    }
                //}

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
