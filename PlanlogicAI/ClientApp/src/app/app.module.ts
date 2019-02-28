import { BrowserModule } from '@angular/platform-browser';
import { NgModule, ErrorHandler } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
//import { AppErrorHandler } from './app.error-handler';

import { ToastyModule } from 'ng2-toasty';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
//import { DataTablesModule } from 'angular-datatables';
import { BusyModule, BusyConfig } from 'angular2-busy';
import { AngularFontAwesomeModule } from 'angular-font-awesome';
import { ChartsModule } from 'ng2-charts';
import { DragulaModule } from "ng2-dragula/ng2-dragula";
import { DragulaService } from 'ng2-dragula/components/dragula.provider';
import { CurrencyMaskModule } from 'ng2-currency-mask';
import { NgxSelectModule } from 'ngx-select-ex';

import { NeedsAnalysisService } from './services/needs-analysis.service';
import { NotifService } from './services/notif-service.service';
import { ClientService } from './services/client.service';
import { CashFlowService } from './services/cashFlow.service';
import { CommonService } from './services/common.service';
import { ProjectionService } from './services/projections.service';
import { FinancialAssetService } from './services/financialAsset.service';
import { LifestyleAssetService } from './services/lifestyleAsset.service';
import { PropertyService } from './services/property.service';
import { InvestmentService } from './services/investment.service';
import { SuperService } from './services/super.service';
import { LiabilityService } from './services/liability.service';
import { PensionService } from './services/pension.service';
import { SuperAssumptionsService } from './services/superAssumptions.service';
import { PensionAssumptionsService } from './services/pensionAssumptions.service';
import { CentrelinkAssumptionsService } from './services/centrelinkAssumptions.service';
import { ProductService } from './services/product.service';
import { FeeService } from './services/fee.service';
import { InvestmentFundService } from './services/investment-fund.service';
import { CurrentPortfolioService } from './services/current-portfolio.service';
import { CurrentPortfolioFundService } from './services/current-portfolio-fund.service';
import { ProductSwitchingService } from './services/product-switching.service';
import { InsuranceSwitchingService } from './services/insurance-switching.service';
import { NewProductService } from './services/new-product.service';
import { AlternativeProductService } from './services/alternative-product.service';
import { ROPCurrentService } from './services/rop-current.service';

//import { NotificationComponent } from './components/notification/notification.component';
import { AppComponent } from './app.component';
import { NavMenuComponent } from './components/navmenu/navmenu.component';
import { HomeComponent } from './components/home/home.component';
import { CounterComponent } from './components/counter/counter.component';
import { ClientNewComponent } from './components/client-new/client-new.component';
import { ClientListComponent } from './components/client-list/client-list.component';
import { CashflowIncomeComponent } from './components/cashflow-income/cashflow-income.component';
import { CashflowExpenditureComponent } from './components/cashflow-expenditure/cashflow-expenditure.component';
import { CashflowProjectionComponent } from './components/cashflow-projection/cashflow-projection.component';
import { TaxationProjectionComponent } from './components/taxation-projection/taxation-projection.component';
import { CurrentPositionComponent } from './components/current-position/current-position.component';
import { AssetsComponent } from './components/assets/assets.component';
import { LiabilityComponent } from './components/liability/liability.component';
import { CurrentProjectionsComponent } from './components/current-projections/current-projections.component';
import { CurrentOverviewComponent } from './components/current-overview/current-overview.component';
import { ProductCurrentPositionComponent } from './components/product-current-position/product-current-position.component';
import { CurrentPortfolioComponent } from './components/current-portfolio/current-portfolio.component';
import { ProductSwitchingComponent } from './components/product-switching/product-switching.component';
import { NeedsAnalysisComponent } from './components/needs-analysis/needs-analysis.component';
import { InsuranceComponent } from './components/insurance/insurance.component';

import { ExponentialStrengthPipe,MinusSignToParens, UnutilizedProductPipe } from './pipes/exponential-strength';
import { KeysPipe } from './pipes/key-value';
import { FilterArrayPipe, FilterFurtherArrayPipe, FilteHiderArrayPipe, FilterFeePipe, FilterCostTypePipe, FilterFurtherAllowZeroArrayPipe } from './pipes/filter-array';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MaterialModule } from './material.module';
import { MomentDateModule } from '@angular/material-moment-adapter';
import { NgxMatSelectSearchModule } from 'ngx-mat-select-search';

export function getBusyConfig() {
    return new BusyConfig({
        message: 'Please wait ...',
        backdrop: false,
        delay: 300,
        minDuration: 800,
        wrapperClass: 'ng-busy'
    });
}

@NgModule({
  declarations: [
        AppComponent,
        NavMenuComponent,
        CounterComponent,
        HomeComponent,
        //NotificationComponent,
        ClientNewComponent,
        ClientListComponent,
        CashflowIncomeComponent,
        CashflowExpenditureComponent,
        CashflowProjectionComponent,
        ExponentialStrengthPipe,
        MinusSignToParens,
        KeysPipe,
        FilterArrayPipe,
        FilterFurtherArrayPipe,
        FilterFurtherAllowZeroArrayPipe,
        FilteHiderArrayPipe,
        FilterFeePipe,
        FilterCostTypePipe,
        UnutilizedProductPipe,
        TaxationProjectionComponent,
        CurrentPositionComponent,
        AssetsComponent,
        LiabilityComponent,
        CurrentProjectionsComponent,
        CurrentOverviewComponent,
        ProductCurrentPositionComponent,
        CurrentPortfolioComponent,
        ProductSwitchingComponent,
        NeedsAnalysisComponent,
        InsuranceComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    NgbModule.forRoot(),
        CommonModule,
    HttpClientModule,
    BrowserAnimationsModule,
    MaterialModule,
        FormsModule,
        ToastyModule.forRoot(),
        ReactiveFormsModule,
        RouterModule.forRoot([
            { path: '', redirectTo: 'clients/new', pathMatch: 'full' },
            { path: 'home', component: HomeComponent },
           // { path: 'basic-details', component: ClientListComponent },
            {
                path: 'counter', component: CounterComponent,
                children: [
                    { path: '', redirectTo: 'basic-details', pathMatch: 'full' },
                    { path: 'current-position', component: CurrentPositionComponent },
                    { path: 'basic-details', component: ClientListComponent },
                    { path: 'current-overview', component: CurrentOverviewComponent },
                    { path: 'current-projections/:id', component: CurrentProjectionsComponent },
                    { path: 'product-current-position', component: ProductCurrentPositionComponent },
                    { path: 'current-portfolio', component: CurrentPortfolioComponent },
                    { path: 'product-switching/:id', component: ProductSwitchingComponent },
                    { path: 'product-switching/:id', component: ProductSwitchingComponent },
                    { path: 'needs-analysis', component: NeedsAnalysisComponent },
                    { path: 'insurance', component: InsuranceComponent }
                    //{ path: 'replacement-preview', component: ReplacementPreviewComponent }
                ]
            },

            { path: 'clients/new', component: ClientNewComponent },
            { path: 'clients/:id', component: ClientNewComponent },
            { path: 'clients', component: ClientListComponent },
            { path: '**', redirectTo: 'home' }
        ])

        , BsDatepickerModule.forRoot()
        //, DataTablesModule
        , AngularFontAwesomeModule
        , ChartsModule
        , DragulaModule
        , NgxSelectModule
        , CurrencyMaskModule
    , MomentDateModule
    , NgxMatSelectSearchModule

  ],

  providers: [ClientService, CashFlowService, NotifService, CommonService, ProjectionService, FinancialAssetService, LifestyleAssetService, PropertyService, InvestmentService, SuperService, LiabilityService, PensionService, SuperAssumptionsService, PensionAssumptionsService, DragulaService, CentrelinkAssumptionsService, ProductService, FeeService, InvestmentFundService, CurrentPortfolioService, CurrentPortfolioFundService, ProductSwitchingService, InsuranceSwitchingService, NewProductService, AlternativeProductService, ROPCurrentService, NeedsAnalysisService],
  bootstrap: [AppComponent]
})
export class AppModule{ }
