## Winfrm Application
## ©®™

## Libs/pandata.aidb
- Bank
- Categories
- Settings
- 
## Libs/usbanks.aidb
- BankInfo

## Libs/zulu.aidb
- city_data

## Company/demo_company.aidb
- Accounts
- Company
- Transactions
- UserCategories
- BatchIDs

## Screen Images
- Account Screen
- Bank Screen
- Company
- Import Screen
- Report Screen
- Settings Screen
- Transaction Screen

## Dialog Images
- AddBank
- AddNAICS
- Authorize
- Category
- CustomMessage
- Login
- Message
- SplitTransactions


🧾💰💻

AccountManager acctMgr = new AccountManager();
- acctMgr.CreateAccountsTable();
x acctMgr.GetAccountsList();
x acctMgr.GetAccountID();

BankManager bankMgr = new BankManager();
- bankMgr.CreateBankTableIfNotExists();
x bankMgr.GetBankJsonReport();
x bankMgr.GetBanksList();
x bankMgr.InsertTestBank();
x bankMgr.UpdateBank(new BankInfo());
x bankMgr.InsertBank(new BankInfo());
x bankMgr.GetBanks();

BatchManager batchMgr = new BatchManager();
batchMgr.UpdateBatch(new DataModel.BatchInfo());
batchMgr.SaveBatch(new DataModel.BatchInfo());
batchMgr.GetBatchList(Guid.NewGuid().ToByteArray(), "", "");
batchMgr.UpdateBatch(new DataModel.BatchInfo());
batchMgr.CreateBatchTable();

CategoryMasterTableManager categoryMgr = new CategoryMasterTableManager();
categoryMgr.AddCategory("", "", "", false, "");
categoryMgr.LoadDefaultCategories();
categoryMgr.DeleteCategoryByName("");
categoryMgr.CopyAllCategoriesToUserCategories();
categoryMgr.AddCategory("", "", "", false, "");
categoryMgr.GetAllCategories();

CompanyCategoryManager companyCategoryMgr = new CompanyCategoryManager();
companyCategoryMgr.InsertUserCategory("", "", "", false, "");
companyCategoryMgr.InsertUserCategoriesBulk(new List<CompanyCategoryManager.CategoryInfo>());

CompanyManager companyMgr = new CompanyManager();
- companyMgr.CreateAndInsertCompany();
x companyMgr.CreateNewCompany(new Company());
x companyMgr.UpdateCompany(new Company());
x companyMgr.GetCompanies();

//ExpenseCategorySummary

SplitDetailManager splitDetailMgr = new SplitDetailManager();
splitDetailMgr.AddSplitDetail(Guid.NewGuid().ToByteArray(), "", (double)0.00, "");
splitDetailMgr.CreateSplitDetailsTable();
splitDetailMgr.DeleteSplitDetail(Guid.NewGuid().ToByteArray());
splitDetailMgr.GetSplitDetails();


