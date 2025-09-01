using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Authentication.ExtendedProtection;
using System.Text.RegularExpressions;
using System.Threading;
//using System.Web.Script.Serialization;
using System.Windows.Forms;
//using YouTubeSearch;
//using QuickDL;
using System.Configuration;
//using static RumbleEmbedVideoURLExtractor;

//strQuery = "SELECT [selected] " +
//    ",[csid] " +
//    ",[name] " +
//    ",[description] " +
//    ",[cstring] " +
//"FROM [Connections] ";

namespace AiNetStudio.Models
{
    public class AppAds
    {
        public string adId { get; set; }
        public string appId { get; set; }
        public string distributorId { get; set; }
        public string description { get; set; }
        public string url { get; set; }
        public bool bIFrame { get; set; }
        public DateTime beginDate { get; set; }
        public DateTime endDate { get; set; }
        public bool closed = false;

        // New property for storing the image
        public byte[] adImage { get; set; }
    }


    public class RumbleVideoInfo
    {
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public string URL { get; set; }
        public string EmbedURL { get; set; }
        public string LinkValue { get; set; }
        public string Image { get; set; }
        public string Duration { get; set; }
    }

    //public class VideoExtractionResult
    //{
    //    public string VideoID { get; set; }
    //    public Uri EmbedUrl { get; set; }
    //    public UrlExtractorError? Error { get; set; }

    //    public bool IsSuccess => Error == null;
    //}

    public class ConnectionStrings
    {
        private int _csid;
        private string _name = string.Empty;
        private string _description = string.Empty;
        private string _cstring = string.Empty;
        private int _selected = 0;

        public int csid
        {
            get
            {
                return _csid;
            }
            set
            {
                _csid = value;
            }
        }

        public string name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        public string description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
            }
        }

        public string cstring
        {
            get
            {
                return _cstring;
            }
            set
            {
                _cstring = value;
            }
        }

        public int selected
        {
            get
            {
                return _selected;
            }
            set
            {
                _selected = value;
            }
        }

    }

    public class FeedCategories : BaseEntity
    {
        public string category { get; set; }
        public string count { get; set; }
    }

    public class FeedSubCategories : BaseEntity
    {
        public string subcategory { get; set; }
        public string count { get; set; }
    }

    public class FeedGroupCategories : BaseEntity
    {
        public string groupcategory { get; set; }
        public string count { get; set; }
    }

    public class MovieCategories : BaseEntity
    {
        public string moviecategory { get; set; }
        public string count { get; set; }
    }

    public abstract class BaseEntity
    {
    }

    public class RSSFeed : BaseEntity
    {
        private string? _feedid;
        private string _category = string.Empty;
        private string _subcategory = string.Empty;
        private string _catsub = string.Empty;
        private string _groupcategory = string.Empty;
        private string _title = string.Empty;
        private string _author = string.Empty;
        private string _shortDescription = string.Empty;
        private string _description = string.Empty;
        private string _bodyLinks = string.Empty;
        private string _link = string.Empty;
        private string _linkType = string.Empty;
        private string _linkValue = string.Empty;
        private string _image = string.Empty;
        private int _rank = 0;
        private DateTime _publishedDate = DateTime.UtcNow;
        private DateTime _beginDate = DateTime.UtcNow;
        private DateTime _endDate = DateTime.UtcNow;

        private string _warnings = string.Empty;
        private string _sideeffects = string.Empty;
        private string _dosage = string.Empty;

        // Private fields for smallint columns
        private short _anticoagulant;
        private short _carcinogenic;
        private short _hypoglycemic;
        private short _liverdamage;
        private short _kidneydamage;
        private short _neuropathy;
        private short _nootropic;

        private short _closed;
        private short _carousel;
        private short _showvideo;

        private string _city = string.Empty;
        private string _state = string.Empty;
        private string _postalCode = string.Empty;
        private string _country = string.Empty;
        private string _areaCode = string.Empty;

        private string _carousel_caption = string.Empty;
        private string _moviecategory = string.Empty;
        private string _duration = string.Empty;

        private int _totalRows = 0;

        public string bodyLinks
        {
            get
            {
                return _bodyLinks;
            }
            set
            {
                _bodyLinks = value;
            }
        }

        public string FeedId
        {
            get
            {
                return _feedid;
            }
            set
            {
                _feedid = value;
            }
        }

        public string category
        {
            get
            {
                return _category;
            }
            set
            {
                _category = value;
            }
        }

        public string subcategory
        {
            get
            {
                return _subcategory;
            }
            set
            {
                _subcategory = value;
            }
        }

        public string catsub
        {
            get
            {
                return _catsub;
            }
            set
            {
                _catsub = value;
            }
        }

        public string groupcategory
        {
            get
            {
                return _groupcategory;
            }
            set
            {
                _groupcategory = value;
            }
        }

        public string title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
            }
        }

        public string author
        {
            get
            {
                return _author;
            }
            set
            {
                _author = value;
            }
        }

        public string shortDescription
        {
            get
            {
                return _shortDescription;
            }
            set
            {
                _shortDescription = value;
            }
        }

        public string description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
            }
        }

        public string link
        {
            get
            {
                return _link;
            }
            set
            {
                _link = value;
            }
        }

        public string linkType
        {
            get
            {
                return _linkType;
            }
            set
            {
                _linkType = value;
            }
        }

        public string linkValue
        {
            get
            {
                return _linkValue;
            }
            set
            {
                _linkValue = value;
            }
        }

        public string image
        {
            get
            {
                return _image;
            }
            set
            {
                _image = value;
            }
        }

        public DateTime publishedDate
        {
            get
            {
                return _publishedDate;
            }
            set
            {
                _publishedDate = value;
            }
        }

        public int rank
        {
            get
            {
                return _rank;
            }
            set
            {
                _rank = value;
            }
        }

        public DateTime beginDate
        {
            get
            {
                return _beginDate;
            }
            set
            {
                _beginDate = value;
            }
        }

        public DateTime endDate
        {
            get
            {
                return _endDate;
            }
            set
            {
                _endDate = value;
            }
        }

        public int totalRows
        {
            get
            {
                return _totalRows;
            }
            set
            {
                _totalRows = value;
            }
        }

        public string warnings
        {
            get
            {
                return _warnings;
            }
            set
            {
                _warnings = value;
            }
        }

        //private string _sideeffects = string.Empty;
        public string sideeffects
        {
            get
            {
                return _sideeffects;
            }
            set
            {
                _sideeffects = value;
            }
        }

        //private string _dosage = string.Empty;
        public string dosage
        {
            get
            {
                return _dosage;
            }
            set
            {
                _dosage = value;
            }
        }

        //private bool _anticoagulant = false;
        public bool anticoagulant
        {
            get { return _anticoagulant != 0; }
            set { _anticoagulant = value ? (short)1 : (short)0; }
        }

        //private bool _carcinogenic = false;
        //public int carcinogenic
        //{
        //    get
        //    {
        //        return _carcinogenic;
        //    }
        //    set
        //    {
        //        _carcinogenic = value;
        //    }
        //}
        public bool carcinogenic
        {
            get { return _carcinogenic != 0; }
            set { _carcinogenic = value ? (short)1 : (short)0; }
        }

        //private bool _hypoglycemic = false;
        //public int hypoglycemic
        //{
        //    get
        //    {
        //        return _hypoglycemic;
        //    }
        //    set
        //    {
        //        _hypoglycemic = value;
        //    }
        //}
        public bool hypoglycemic
        {
            get { return _hypoglycemic != 0; }
            set { _hypoglycemic = value ? (short)1 : (short)0; }
        }


        //private bool _liverdamage = false;
        //public int liverdamage
        //{
        //    get
        //    {
        //        return _liverdamage;
        //    }
        //    set
        //    {
        //        _liverdamage = value;
        //    }
        //}
        public bool liverdamage
        {
            get { return _liverdamage != 0; }
            set { _liverdamage = value ? (short)1 : (short)0; }
        }

        //private bool _kidneydamage = false;
        //public int kidneydamage
        //{
        //    get
        //    {
        //        return _kidneydamage;
        //    }
        //    set
        //    {
        //        _kidneydamage = value;
        //    }
        //}
        public bool kidneydamage
        {
            get { return _kidneydamage != 0; }
            set { _kidneydamage = value ? (short)1 : (short)0; }
        }

        //public int neuropathy
        //{
        //    get
        //    {
        //        return _neuropathy;
        //    }
        //    set
        //    {
        //        _neuropathy = value;
        //    }
        //}
        public bool neuropathy
        {
            get { return _neuropathy != 0; }
            set { _neuropathy = value ? (short)1 : (short)0; }
        }

        //public int nootropic
        //{
        //    get
        //    {
        //        return _nootropic;
        //    }
        //    set
        //    {
        //        _nootropic = value;
        //    }
        //}
        public bool nootropic
        {
            get { return _nootropic != 0; }
            set { _nootropic = value ? (short)1 : (short)0; }
        }

        public string city
        {
            get
            {
                return _city;
            }
            set
            {
                _city = value;
            }
        }

        public string state
        {
            get
            {
                return _state;
            }
            set
            {
                _state = value;
            }
        }

        public string postalCode
        {
            get
            {
                return _postalCode;
            }
            set
            {
                _postalCode = value;
            }
        }

        public string country
        {
            get
            {
                return _country;
            }
            set
            {
                _country = value;
            }
        }

        public string areaCode
        {
            get
            {
                return _areaCode;
            }
            set
            {
                _areaCode = value;
            }
        }


        //public bool closed
        //{
        //    get
        //    {
        //        return _closed;
        //    }
        //    set
        //    {
        //        _closed = value;
        //    }
        //}
        public bool closed
        {
            get { return _closed != 0; }
            set { _closed = value ? (short)1 : (short)0; }
        }

        //public bool carousel
        //{
        //    get
        //    {
        //        return _carousel;
        //    }
        //    set
        //    {
        //        _carousel = value;
        //    }
        //}
        public bool carousel
        {
            get { return _carousel != 0; }
            set { _carousel = value ? (short)1 : (short)0; }
        }

        public string carousel_caption
        {
            get
            {
                return _carousel_caption;
            }
            set
            {
                _carousel_caption = value;
            }
        }
        public bool showvideo
        {
            get { return _showvideo != 0; }
            set { _showvideo = value ? (short)1 : (short)0; }
        }

        public string moviecategory
        {
            get
            {
                return _moviecategory;
            }
            set
            {
                _moviecategory = value;
            }
        }

        public string duration
        {
            get
            {
                return _duration;
            }
            set
            {
                _duration = value;
            }
        }

    }

    public class SBUser : BaseEntity
    {
        private string _userid;
        //private string _clientid;
        private string _appids = string.Empty;
        private string _username = string.Empty;
        private string _password = string.Empty;
        //private string _PasswordHash = string.Empty;
        //private string _salt = string.Empty;
        //private string _roles = string.Empty;
        //private string _sysop;
        //private string _admin;
        //private string _buyer;
        //private string _client;
        //private string _guest;
        //private string _locked;
        //private string _blocked;
        //private string _closed;
        //private string _IPAddress;
        //private string _Browser;
        private string _firstName = string.Empty;
        private string _lastName = string.Empty;
        private string _email = string.Empty;
        //private string _PostalCode = string.Empty;
        //private string _Country = string.Empty;
        //private string _State = string.Empty;
        //private string _City = string.Empty;
        //private string _Phone = string.Empty;
        private string _token = string.Empty;

        public string userid { get { return _userid; } set { _userid = value; } }
        public string appids { get { return _appids; } set { _appids = value; } }
        public string username { get { return _username; } set { _username = value; } }
        public string password { get { return _password; } set { _password = value; } }
        public string firstName { get { return _firstName; } set { _firstName = value; } }
        public string lastName { get { return _lastName; } set { _lastName = value; } }
        public string email { get { return _email; } set { _email = value; } }
        public string token { get { return _token; } set { _token = value; } }
    }

    public class Customer : BaseEntity
    {
        private string _id;
        private string _company = string.Empty;
        private string _contactName = string.Empty;
        private string _contactTitle = string.Empty;
        private string _address = string.Empty;
        private string _city = string.Empty;
        private string _postalCode = string.Empty;
        private string _country = string.Empty;
        private string _phone = string.Empty;
        private string _fax = string.Empty;

        public string ID
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }

        public string Company
        {
            get
            {
                return _company;
            }
            set
            {
                _company = value;
            }
        }

        public string ContactName
        {
            get
            {
                return _contactName;
            }
            set
            {
                _contactName = value;
            }
        }

        public string ContactTitle
        {
            get
            {
                return _contactTitle;
            }
            set
            {
                _contactTitle = value;
            }
        }

        public string Address
        {
            get
            {
                return _address;
            }
            set
            {
                _address = value;
            }
        }

        public string City
        {
            get
            {
                return _city;
            }
            set
            {
                _city = value;
            }
        }

        public string PostalCode
        {
            get
            {
                return _postalCode;
            }
            set
            {
                _postalCode = value;
            }
        }

        public string Country
        {
            get
            {
                return _country;
            }
            set
            {
                _country = value;
            }
        }

        public string Phone
        {
            get
            {
                return _phone;
            }
            set
            {
                _phone = value;
            }
        }
    }

    public class Category : BaseEntity
    {
        private int _id;
        private string _name = string.Empty;

        public int ID
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }
    }

    public class AppDownloads : BaseEntity
    {
        private string _appName = string.Empty;
        private string _january = string.Empty;
        private string _february = string.Empty;
        private string _march = string.Empty;
        private string _april = string.Empty;
        private string _may = string.Empty;
        private string _june = string.Empty;
        private string _july = string.Empty;
        private string _august = string.Empty;
        private string _september = string.Empty;
        private string _october = string.Empty;
        private string _november = string.Empty;
        private string _december = string.Empty;
        private string _total = string.Empty;

        public string appName { get { return _appName; } set { _appName = value; } }
        public string january { get { return _january; } set { _january = value; } }
        public string february { get { return _february; } set { _february = value; } }
        public string march { get { return _march; } set { _march = value; } }
        public string april { get { return _april; } set { _april = value; } }
        public string may { get { return _may; } set { _may = value; } }
        public string june { get { return _june; } set { _june = value; } }
        public string july { get { return _july; } set { _july = value; } }
        public string august { get { return _august; } set { _august = value; } }
        public string september { get { return _september; } set { _september = value; } }
        public string october { get { return _october; } set { _october = value; } }
        public string november { get { return _november; } set { _november = value; } }
        public string december { get { return _december; } set { _december = value; } }
        public string total { get { return _total; } set { _total = value; } }
    }

    public class CatData : BaseEntity
    {
        private string _category = string.Empty;
        private string _total = string.Empty;
        private string _per = string.Empty;

        public string category { get { return _category; } set { _category = value; } }
        public string total { get { return _total; } set { _total = value; } }
        public string per { get { return _per; } set { _per = value; } }
    }

    public class AdUrlMobile : BaseEntity
    {
        //ALTER PROCEDURE[dbo].[usp_AddAdUrl]
        //(
        //       @description varchar(50)
        //		,@url varchar(50)
        //		,@distId varchar(50)
        //		,@beginDate datetime
        //      ,@endDate datetime
        //		,@closed bit
        //   )  

        //SELECT[adId]
        //	,[url]
        //	,[distId]
        //	,[iframe]
        //	,[description]
        //FROM[dbo].[AdUrls]
        //WHERE[closed] != 1

        private string _adId = string.Empty;
        private string _url = string.Empty;
        private string _distId = string.Empty;
        private Boolean _iframe = false;
        private string _description = string.Empty;

        public string AdId
        {
            get
            {
                return _adId;
            }
            set
            {
                _adId = value;
            }
        }

        public string Url
        {
            get
            {
                return _url;
            }
            set
            {
                _url = value;
            }
        }

        public string DistId
        {
            get
            {
                return _distId;
            }
            set
            {
                _distId = value;
            }
        }

        public Boolean iFrame
        {
            get
            {
                return _iframe;
            }
            set
            {
                _iframe = value;
            }
        }

        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
            }
        }
    }

    public class AdUrl : BaseEntity
    {
        //ALTER PROCEDURE[dbo].[usp_AddAdUrl]
        //(
        //       @description varchar(50)
        //		,@url varchar(50)
        //		,@distId varchar(50)
        //		,@beginDate datetime
        //      ,@endDate datetime
        //		,@closed bit
        //   )  

        private string _adId = string.Empty;
        private string _description = string.Empty;
        private string _url = string.Empty;
        private string _distId = string.Empty;
        private Boolean _iframe = false;
        private DateTime _beginDate = DateTime.UtcNow;
        private DateTime _endDate = DateTime.UtcNow;
        private Boolean _closed = false;

        public string AdId
        {
            get
            {
                return _adId;
            }
            set
            {
                _adId = value;
            }
        }

        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
            }
        }

        public string Url
        {
            get
            {
                return _url;
            }
            set
            {
                _url = value;
            }
        }

        public string DistId
        {
            get
            {
                return _distId;
            }
            set
            {
                _distId = value;
            }
        }

        public Boolean iFrame
        {
            get
            {
                return _iframe;
            }
            set
            {
                _iframe = value;
            }
        }

        public DateTime BeginDate
        {
            get
            {
                return _beginDate;
            }
            set
            {
                _beginDate = value;
            }
        }

        public DateTime EndDate
        {
            get
            {
                return _endDate;
            }
            set
            {
                _endDate = value;
            }
        }

        public Boolean Closed
        {
            get
            {
                return _closed;
            }
            set
            {
                _closed = value;
            }
        }

    }

    public class LinkTypes : BaseEntity
    {
        //private int _id;
        private string _linkType = string.Empty;
        private string _linkDescription = string.Empty;

        public string LinkType
        {
            get
            {
                return _linkType;
            }
            set
            {
                _linkType = value;
            }
        }

        public string LinkDescription
        {
            get
            {
                return _linkDescription;
            }
            set
            {
                _linkDescription = value;
            }
        }
    }

    public class Product : BaseEntity
    {
        private int _id;
        private string _name = string.Empty;
        private int _categoryID;
        private string _quantityPerUnit = string.Empty;
        private decimal _unitPrice;
        private short _unitsInStock;
        private short _unitsOnOrder;
        private bool _discontinued;

        public int ID
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        public int CategoryID
        {
            get
            {
                return _categoryID;
            }
            set
            {
                _categoryID = value;
            }
        }

        public string QuantityPerUnit
        {
            get
            {
                return _quantityPerUnit;
            }
            set
            {
                _quantityPerUnit = value;
            }
        }

        public decimal UnitPrice
        {
            get
            {
                return _unitPrice;
            }
            set
            {
                _unitPrice = value;
            }
        }

        public short UnitsInStock
        {
            get
            {
                return _unitsInStock;
            }
            set
            {
                _unitsInStock = value;
            }
        }

        public short UnitsOnOrder
        {
            get
            {
                return _unitsOnOrder;
            }
            set
            {
                _unitsOnOrder = value;
            }
        }

        public bool Discontinued
        {
            get
            {
                return _discontinued;
            }
            set
            {
                _discontinued = value;
            }
        }
    }

    public class PagedResult<T> where T : BaseEntity
    {
        private int _total;
        private List<T> _rows;

        public int Total
        {
            get
            {
                return _total;
            }
            set
            {
                _total = value;
            }
        }

        public List<T> Rows
        {
            get
            {
                return _rows;
            }
            set
            {
                _rows = value;
            }
        }
    }

    public class SBAMaps : BaseEntity
    {
        private string _stationid = string.Empty;
        private string _callsign = string.Empty;
        private string _smediatype = string.Empty;
        private string _saff1 = string.Empty;
        private string _saff2 = string.Empty;
        private string _test = string.Empty;
        private string _bought = string.Empty;
        private string _nationalstation = string.Empty;
        private string _scity = string.Empty;
        private string _sregion = string.Empty;
        private string _szip = string.Empty;
        private string _scountry = string.Empty;
        private string _latitude = string.Empty;
        private string _longitude = string.Empty;
        private string _sales = string.Empty;
        private string _pi = string.Empty;
        private string _air_total = string.Empty;
        private string _air_rank = string.Empty;
        private string _mg_total = string.Empty;
        private string _mg_rank = string.Empty;
        private string _pi_total = string.Empty;
        private string _pi_rank = string.Empty;
        private string _gpi_total = string.Empty;
        private string _gpi_rank = string.Empty;
        private string _mgross_total = string.Empty;
        private string _mgross_rank = string.Empty;
        private string _sgross_total = string.Empty;
        private string _sgross_rank = string.Empty;

        public string stationid
        {
            get
            {
                return _stationid;
            }
            set
            {
                _stationid = value;
            }
        }

        public string callsign
        {
            get
            {
                return _callsign;
            }
            set
            {
                _callsign = value;
            }
        }

        public string mediatype
        {
            get
            {
                return _smediatype;
            }
            set
            {
                _smediatype = value;
            }
        }

        public string aff1
        {
            get
            {
                return _saff1;
            }
            set
            {
                _saff1 = value;
            }
        }

        public string aff2
        {
            get
            {
                return _saff2;
            }
            set
            {
                _saff2 = value;
            }
        }

        public string test
        {
            get
            {
                return _test;
            }
            set
            {
                _test = value;
            }
        }

        public string bought
        {
            get
            {
                return _bought;
            }
            set
            {
                _bought = value;
            }
        }

        public string nationalstation
        {
            get
            {
                return _nationalstation;
            }
            set
            {
                _nationalstation = value;
            }
        }

        public string city
        {
            get
            {
                return _scity;
            }
            set
            {
                _scity = value;
            }
        }

        public string region
        {
            get
            {
                return _sregion;
            }
            set
            {
                _sregion = value;
            }
        }

        public string zip
        {
            get
            {
                return _szip;
            }
            set
            {
                _szip = value;
            }
        }

        public string country
        {
            get
            {
                return _scountry;
            }
            set
            {
                _scountry = value;
            }
        }

        public string latitude
        {
            get
            {
                return _latitude;
            }
            set
            {
                _latitude = value;
            }
        }

        public string longitude
        {
            get
            {
                return _longitude;
            }
            set
            {
                _longitude = value;
            }
        }

        public string sales
        {
            get
            {
                return _sales;
            }
            set
            {
                _sales = value;
            }
        }

        public string pi
        {
            get
            {
                return _pi;
            }
            set
            {
                _pi = value;
            }
        }

        public string air_total
        {
            get
            {
                return _air_total;
            }
            set
            {
                _air_total = value;
            }
        }

        public string air_rank
        {
            get
            {
                return _air_rank;
            }
            set
            {
                _air_rank = value;
            }
        }

        public string mg_total
        {
            get
            {
                return _mg_total;
            }
            set
            {
                _mg_total = value;
            }
        }

        public string mg_rank
        {
            get
            {
                return _mg_rank;
            }
            set
            {
                _mg_rank = value;
            }
        }

        public string pi_total
        {
            get
            {
                return _pi_total;
            }
            set
            {
                _pi_total = value;
            }
        }

        public string pi_rank
        {
            get
            {
                return _pi_rank;
            }
            set
            {
                _pi_rank = value;
            }
        }

        public string gpi_total
        {
            get
            {
                return _gpi_total;
            }
            set
            {
                _gpi_total = value;
            }
        }

        public string gpi_rank
        {
            get
            {
                return _gpi_rank;
            }
            set
            {
                _gpi_rank = value;
            }
        }

        public string mgross_total
        {
            get
            {
                return _mgross_total;
            }
            set
            {
                _mgross_total = value;
            }
        }

        public string mgross_rank
        {
            get
            {
                return _mgross_rank;
            }
            set
            {
                _mgross_rank = value;
            }
        }

        public string sgross_total
        {
            get
            {
                return _sgross_total;
            }
            set
            {
                _sgross_total = value;
            }
        }

        public string sgross_rank
        {
            get
            {
                return _sgross_rank;
            }
            set
            {
                _sgross_rank = value;
            }
        }

    }

    public class MAPS : BaseEntity
    {
        public string _callsign = string.Empty;
        public string _city = string.Empty;
        public string _region = string.Empty;
        public string _zip = string.Empty;
        public string _country = string.Empty;
        public string _latitude = string.Empty;
        public string _longitude = string.Empty;
        public string _DistanceInMiles = string.Empty;

        public string callsign
        {
            get
            {
                return _callsign;
            }
            set
            {
                _callsign = value;
            }
        }

        public string city
        {
            get
            {
                return _city;
            }
            set
            {
                _city = value;
            }
        }

        public string region
        {
            get
            {
                return _region;
            }
            set
            {
                _region = value;
            }
        }

        public string zip
        {
            get
            {
                return _zip;
            }
            set
            {
                _zip = value;
            }
        }

        public string country
        {
            get
            {
                return _country;
            }
            set
            {
                _country = value;
            }
        }

        public string latitude
        {
            get
            {
                return _latitude;
            }
            set
            {
                _latitude = value;
            }
        }

        public string longitude
        {
            get
            {
                return _longitude;
            }
            set
            {
                _longitude = value;
            }
        }

        public string DistanceInMiles
        {
            get
            {
                return _DistanceInMiles;
            }
            set
            {
                _DistanceInMiles = value;
            }
        }

    }

    public class SBAStats : BaseEntity
    {
        private string _stationid = string.Empty;
        public string stationid { get { return _stationid; } set { _stationid = value; } }

        private string _callsign = string.Empty;
        public string callsign { get { return _callsign; } set { _callsign = value; } }

        private string _dash = string.Empty;
        public string dash { get { return _dash; } set { _dash = value; } }

        private string _fcc_fac_callsign = string.Empty;
        public string fcc_fac_callsign { get { return _fcc_fac_callsign; } set { _fcc_fac_callsign = value; } }

        private string _fcc_fac_id = string.Empty;
        public string fcc_fac_id { get { return _fcc_fac_id; } set { _fcc_fac_id = value; } }

        private string _fcc_fac_service = string.Empty;
        public string fcc_fac_service { get { return _fcc_fac_service; } set { _fcc_fac_service = value; } }

        private string _mediatype = string.Empty;
        public string mediatype { get { return _mediatype; } set { _mediatype = value; } }

        private string _aff1 = string.Empty;
        public string aff1 { get { return _aff1; } set { _aff1 = value; } }

        private string _aff2 = string.Empty;
        public string aff2 { get { return _aff2; } set { _aff2 = value; } }

        private string _timezone = string.Empty;
        public string timezone { get { return _timezone; } set { _timezone = value; } }

        private string _tapeformat = string.Empty;
        public string tapeformat { get { return _tapeformat; } set { _tapeformat = value; } }

        private string _stationformat = string.Empty;
        public string stationformat { get { return _stationformat; } set { _stationformat = value; } }

        private string _test = string.Empty;
        public string test { get { return _test; } set { _test = value; } }

        private string _bought = string.Empty;
        public string bought { get { return _bought; } set { _bought = value; } }

        private string _nationalstation = string.Empty;
        public string nationalstation { get { return _nationalstation; } set { _nationalstation = value; } }

        private string _rstations = string.Empty;
        public string rstations { get { return _rstations; } set { _rstations = value; } }

        private string _repcode = string.Empty;
        public string repcode { get { return _repcode; } set { _repcode = value; } }

        private string _owner = string.Empty;
        public string owner { get { return _owner; } set { _owner = value; } }

        private string _payee = string.Empty;
        public string payee { get { return _payee; } set { _payee = value; } }

        private string _adirank = string.Empty;
        public string adirank { get { return _adirank; } set { _adirank = value; } }

        private string _adimkt = string.Empty;
        public string adimkt { get { return _adimkt; } set { _adimkt = value; } }

        private string _company = string.Empty;
        public string company { get { return _company; } set { _company = value; } }

        private string _address1 = string.Empty;
        public string address1 { get { return _address1; } set { _address1 = value; } }

        private string _address2 = string.Empty;
        public string address2 { get { return _address2; } set { _address2 = value; } }

        private string _city = string.Empty;
        public string city { get { return _city; } set { _city = value; } }

        private string _region = string.Empty;
        public string region { get { return _region; } set { _region = value; } }

        private string _zip = string.Empty;
        public string zip { get { return _zip; } set { _zip = value; } }

        private string _country = string.Empty;
        public string country { get { return _country; } set { _country = value; } }

        private string _tag = string.Empty;
        public string tag { get { return _tag; } set { _tag = value; } }

        private string _TBuys = string.Empty;
        public string TBuys { get { return _TBuys; } set { _TBuys = value; } }

        private string _TSGross = string.Empty;
        public string TSGross { get { return _TSGross; } set { _TSGross = value; } }

        private string _MaxPay = string.Empty;
        public string MaxPay { get { return _MaxPay; } set { _MaxPay = value; } }

        private string _AvgPullRatio = string.Empty;
        public string AvgPullRatio { get { return _AvgPullRatio; } set { _AvgPullRatio = value; } }

        private string _MaxPullRatio = string.Empty;
        public string MaxPullRatio { get { return _MaxPullRatio; } set { _MaxPullRatio = value; } }

        private string _AvgOrders = string.Empty;
        public string AvgOrders { get { return _AvgOrders; } set { _AvgOrders = value; } }

        private string _MaxOrders = string.Empty;
        public string MaxOrders { get { return _MaxOrders; } set { _MaxOrders = value; } }

        private string _MinMGross = string.Empty;
        public string MinMGross { get { return _MinMGross; } set { _MinMGross = value; } }

        private string _MaxMGross = string.Empty;
        public string MaxMGross { get { return _MaxMGross; } set { _MaxMGross = value; } }

        private string _AvgCPO = string.Empty;
        public string AvgCPO { get { return _AvgCPO; } set { _AvgCPO = value; } }

        private string _MinCPO = string.Empty;
        public string MinCPO { get { return _MinCPO; } set { _MinCPO = value; } }

        private string _AvgMargin = string.Empty;
        public string AvgMargin { get { return _AvgMargin; } set { _AvgMargin = value; } }

        private string _MaxMargin = string.Empty;
        public string MaxMargin { get { return _MaxMargin; } set { _MaxMargin = value; } }

        private string _AvgSGross = string.Empty;
        public string AvgSGross { get { return _AvgSGross; } set { _AvgSGross = value; } }

        private string _MaxSGross = string.Empty;
        public string MaxSGross { get { return _MaxSGross; } set { _MaxSGross = value; } }

        private string _MinSGross = string.Empty;
        public string MinSGross { get { return _MinSGross; } set { _MinSGross = value; } }

        private string _BuysRank = string.Empty;
        public string BuysRank { get { return _BuysRank; } set { _BuysRank = value; } }

        private string _TBuysRank = string.Empty;
        public string TBuysRank { get { return _TBuysRank; } set { _TBuysRank = value; } }

        private string _MGrossRank = string.Empty;
        public string MGrossRank { get { return _MGrossRank; } set { _MGrossRank = value; } }

        private string _TMGrossRank = string.Empty;
        public string TMGrossRank { get { return _TMGrossRank; } set { _TMGrossRank = value; } }

        private string _OrdersRank = string.Empty;
        public string OrdersRank { get { return _OrdersRank; } set { _OrdersRank = value; } }

        private string _TOrdersRank = string.Empty;
        public string TOrdersRank { get { return _TOrdersRank; } set { _TOrdersRank = value; } }

        private string _SGrossRank = string.Empty;
        public string SGrossRank { get { return _SGrossRank; } set { _SGrossRank = value; } }

        private string _TSGrossRank = string.Empty;
        public string TSGrossRank { get { return _TSGrossRank; } set { _TSGrossRank = value; } }

        private string _MkgRank = string.Empty;
        public string MkgRank { get { return _MkgRank; } set { _MkgRank = value; } }

        private string _TMkgRank = string.Empty;
        public string TMkgRank { get { return _TMkgRank; } set { _TMkgRank = value; } }

        private string _PiRank = string.Empty;
        public string PiRank { get { return _PiRank; } set { _PiRank = value; } }

        private string _TPiRank = string.Empty;
        public string TPiRank { get { return _TPiRank; } set { _TPiRank = value; } }

        private string _GarRank = string.Empty;
        public string GarRank { get { return _GarRank; } set { _GarRank = value; } }

        private string _TGarRank = string.Empty;
        public string TGarRank { get { return _TGarRank; } set { _TGarRank = value; } }

        private string _Latitude = string.Empty;
        public string Latitude { get { return _Latitude; } set { _Latitude = value; } }

        private string _Longitude = string.Empty;
        public string Longitude { get { return _Longitude; } set { _Longitude = value; } }

        private string _ZIP_CLASS = string.Empty;
        public string ZIP_CLASS { get { return _ZIP_CLASS; } set { _ZIP_CLASS = value; } }

        private string _TZip = string.Empty;
        public string TZip { get { return _TZip; } set { _TZip = value; } }
    }

    public class Stations : BaseEntity
    {
        private string _stationid = string.Empty;
        public string stationid { get { return _stationid; } set { _stationid = value; } }

        private string _callsign = string.Empty;
        public string callsign { get { return _callsign; } set { _callsign = value; } }

        private string _dash = string.Empty;
        public string dash { get { return _dash; } set { _dash = value; } }

        private string _fcc_fac_callsign = string.Empty;
        public string fcc_fac_callsign { get { return _fcc_fac_callsign; } set { _fcc_fac_callsign = value; } }

        private string _fcc_fac_id = string.Empty;
        public string fcc_fac_id { get { return _fcc_fac_id; } set { _fcc_fac_id = value; } }

        private string _fcc_fac_service = string.Empty;
        public string fcc_fac_service { get { return _fcc_fac_service; } set { _fcc_fac_service = value; } }

        private string _mediatype = string.Empty;
        public string mediatype { get { return _mediatype; } set { _mediatype = value; } }

        private string _aff1 = string.Empty;
        public string aff1 { get { return _aff1; } set { _aff1 = value; } }

        private string _aff2 = string.Empty;
        public string aff2 { get { return _aff2; } set { _aff2 = value; } }

        private string _timezone = string.Empty;
        public string timezone { get { return _timezone; } set { _timezone = value; } }

        private string _tapeformat = string.Empty;
        public string tapeformat { get { return _tapeformat; } set { _tapeformat = value; } }

        private string _stationformat = string.Empty;
        public string stationformat { get { return _stationformat; } set { _stationformat = value; } }

        private string _test = string.Empty;
        public string test { get { return _test; } set { _test = value; } }

        private string _bought = string.Empty;
        public string bought { get { return _bought; } set { _bought = value; } }

        private string _nationalstation = string.Empty;
        public string nationalstation { get { return _nationalstation; } set { _nationalstation = value; } }

        private string _rstations = string.Empty;
        public string rstations { get { return _rstations; } set { _rstations = value; } }

        private string _repcode = string.Empty;
        public string repcode { get { return _repcode; } set { _repcode = value; } }

        private string _owner = string.Empty;
        public string owner { get { return _owner; } set { _owner = value; } }

        private string _payee = string.Empty;
        public string payee { get { return _payee; } set { _payee = value; } }

        private string _adirank = string.Empty;
        public string adirank { get { return _adirank; } set { _adirank = value; } }

        private string _adimkt = string.Empty;
        public string adimkt { get { return _adimkt; } set { _adimkt = value; } }

        private string _company = string.Empty;
        public string company { get { return _company; } set { _company = value; } }

        private string _address1 = string.Empty;
        public string address1 { get { return _address1; } set { _address1 = value; } }

        private string _address2 = string.Empty;
        public string address2 { get { return _address2; } set { _address2 = value; } }

        private string _city = string.Empty;
        public string city { get { return _city; } set { _city = value; } }

        private string _region = string.Empty;
        public string region { get { return _region; } set { _region = value; } }

        private string _zip = string.Empty;
        public string zip { get { return _zip; } set { _zip = value; } }

        private string _country = string.Empty;
        public string country { get { return _country; } set { _country = value; } }

        private string _attention = string.Empty;
        public string attention { get { return _attention; } set { _attention = value; } }

        private string _tel1 = string.Empty;
        public string tel1 { get { return _tel1; } set { _tel1 = value; } }

        private string _tel2 = string.Empty;
        public string tel2 { get { return _tel2; } set { _tel2 = value; } }

        private string _email1 = string.Empty;
        public string email1 { get { return _email1; } set { _email1 = value; } }

        private string _email2 = string.Empty;
        public string email2 { get { return _email2; } set { _email2 = value; } }

        private string _url1 = string.Empty;
        public string url1 { get { return _url1; } set { _url1 = value; } }

        private string _pi = string.Empty;
        public string pi { get { return _pi; } set { _pi = value; } }

        private string _notes = string.Empty;
        public string notes { get { return _notes; } set { _notes = value; } }

        private string _udf1 = string.Empty;
        public string udf1 { get { return _udf1; } set { _udf1 = value; } }

        private string _latitude = string.Empty;
        public string latitude { get { return _latitude; } set { _latitude = value; } }

        private string _longitude = string.Empty;
        public string longitude { get { return _longitude; } set { _longitude = value; } }

        private string _tag = string.Empty;
        public string tag { get { return _tag; } set { _tag = value; } }

        private string _TBuys = string.Empty;
        public string TBuys { get { return _TBuys; } set { _TBuys = value; } }

        private string _TSGross = string.Empty;
        public string TSGross { get { return _TSGross; } set { _TSGross = value; } }

        private string _MaxPay = string.Empty;
        public string MaxPay { get { return _MaxPay; } set { _MaxPay = value; } }

        private string _AvgPullRatio = string.Empty;
        public string AvgPullRatio { get { return _AvgPullRatio; } set { _AvgPullRatio = value; } }

        private string _MaxPullRatio = string.Empty;
        public string MaxPullRatio { get { return _MaxPullRatio; } set { _MaxPullRatio = value; } }

        private string _AvgOrders = string.Empty;
        public string AvgOrders { get { return _AvgOrders; } set { _AvgOrders = value; } }

        private string _MaxOrders = string.Empty;
        public string MaxOrders { get { return _MaxOrders; } set { _MaxOrders = value; } }

        private string _MinMGross = string.Empty;
        public string MinMGross { get { return _MinMGross; } set { _MinMGross = value; } }

        private string _MaxMGross = string.Empty;
        public string MaxMGross { get { return _MaxMGross; } set { _MaxMGross = value; } }

        private string _AvgCPO = string.Empty;
        public string AvgCPO { get { return _AvgCPO; } set { _AvgCPO = value; } }

        private string _MinCPO = string.Empty;
        public string MinCPO { get { return _MinCPO; } set { _MinCPO = value; } }

        private string _AvgMargin = string.Empty;
        public string AvgMargin { get { return _AvgMargin; } set { _AvgMargin = value; } }

        private string _MaxMargin = string.Empty;
        public string MaxMargin { get { return _MaxMargin; } set { _MaxMargin = value; } }

        private string _AvgSGross = string.Empty;
        public string AvgSGross { get { return _AvgSGross; } set { _AvgSGross = value; } }

        private string _MaxSGross = string.Empty;
        public string MaxSGross { get { return _MaxSGross; } set { _MaxSGross = value; } }

        private string _MinSGross = string.Empty;
        public string MinSGross { get { return _MinSGross; } set { _MinSGross = value; } }

        private string _BuysRank = string.Empty;
        public string BuysRank { get { return _BuysRank; } set { _BuysRank = value; } }

        private string _TBuysRank = string.Empty;
        public string TBuysRank { get { return _TBuysRank; } set { _TBuysRank = value; } }

        private string _MGrossRank = string.Empty;
        public string MGrossRank { get { return _MGrossRank; } set { _MGrossRank = value; } }

        private string _TMGrossRank = string.Empty;
        public string TMGrossRank { get { return _TMGrossRank; } set { _TMGrossRank = value; } }

        private string _OrdersRank = string.Empty;
        public string OrdersRank { get { return _OrdersRank; } set { _OrdersRank = value; } }

        private string _TOrdersRank = string.Empty;
        public string TOrdersRank { get { return _TOrdersRank; } set { _TOrdersRank = value; } }

        private string _SGrossRank = string.Empty;
        public string SGrossRank { get { return _SGrossRank; } set { _SGrossRank = value; } }

        private string _TSGrossRank = string.Empty;
        public string TSGrossRank { get { return _TSGrossRank; } set { _TSGrossRank = value; } }

        private string _MkgRank = string.Empty;
        public string MkgRank { get { return _MkgRank; } set { _MkgRank = value; } }

        private string _TMkgRank = string.Empty;
        public string TMkgRank { get { return _TMkgRank; } set { _TMkgRank = value; } }

        private string _PiRank = string.Empty;
        public string PiRank { get { return _PiRank; } set { _PiRank = value; } }

        private string _TPiRank = string.Empty;
        public string TPiRank { get { return _TPiRank; } set { _TPiRank = value; } }

        private string _GarRank = string.Empty;
        public string GarRank { get { return _GarRank; } set { _GarRank = value; } }

        private string _TGarRank = string.Empty;
        public string TGarRank { get { return _TGarRank; } set { _TGarRank = value; } }

        private string _Latitude = string.Empty;
        public string Latitude { get { return _Latitude; } set { _Latitude = value; } }

        private string _Longitude = string.Empty;
        public string Longitude { get { return _Longitude; } set { _Longitude = value; } }

        private string _ZIP_CLASS = string.Empty;
        public string ZIP_CLASS { get { return _ZIP_CLASS; } set { _ZIP_CLASS = value; } }

        private string _TZip = string.Empty;
        public string TZip { get { return _TZip; } set { _TZip = value; } }
    }


    public class Breakthrough
    {
        public string ID { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public string Links { get; set; }
        public int Rank { get; set; }

        //public Breakthrough()
        //{
        //}
    }

    public class ZFeedItem
    {
        public string? FeedId { get; set; }
        public string? Category { get; set; }
        public string? SubCategory { get; set; }
        public string? CatSub { get; set; }
        public string? GroupCategory { get; set; }
        public string? MovieCategory { get; set; }
        public int Rank { get; set; }
        public string? Title { get; set; }
        public string? Author { get; set; }
        public string? Link { get; set; }
        public string? LinkType { get; set; }
        public string? LinkValue { get; set; }
        public string? ShortDescription { get; set; }
        public string? Description { get; set; }
        public string? BodyLinks { get; set; }
        public string? Image { get; set; }
        public string? PublishedDate { get; set; }
        public string? Duration { get; set; }
        public string? Tags { get; set; }

        // ✅ Parameterless constructor so you can do "new ZFeedItem()"
        public ZFeedItem() { }

        // ✅ Full constructor for named parameter use in SQL reader
        public ZFeedItem(
            string? FeedId,
            string? Category,
            string? SubCategory,
            string? CatSub,
            string? GroupCategory,
            string? MovieCategory,
            int Rank,
            string? Title,
            string? Author,
            string? Link,
            string? LinkType,
            string? LinkValue,
            string? ShortDescription,
            string? Description,
            string? BodyLinks,
            string? Image,
            string? PublishedDate,
            string? Duration,
            string? Tags)
        {
            this.FeedId = FeedId;
            this.Category = Category;
            this.SubCategory = SubCategory;
            this.CatSub = CatSub;
            this.GroupCategory = GroupCategory;
            this.MovieCategory = MovieCategory;
            this.Rank = Rank;
            this.Title = Title;
            this.Author = Author;
            this.Link = Link;
            this.LinkType = LinkType;
            this.LinkValue = LinkValue;
            this.ShortDescription = ShortDescription;
            this.Description = Description;
            this.BodyLinks = BodyLinks;
            this.Image = Image;
            this.PublishedDate = PublishedDate;
            this.Duration = Duration;
            this.Tags = Tags;
        }
    }





}




