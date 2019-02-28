import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
    name: 'filter'
})
export class FilterArrayPipe implements PipeTransform {
    transform(items: Array<any>, category: string): Array<any> {
        return items.filter(item => item.owner === category);
    }
}

@Pipe({
    name: 'filterFee'
})
export class FilterFeePipe implements PipeTransform {

    transform(items: Array<any>, feeName: string): Array<any> {
        return items.filter(item => item.name === feeName);
    }
} 

@Pipe({
    name: 'filterCostType'
})
export class FilterCostTypePipe implements PipeTransform {
    transform(items: Array<any>, costType: string): Array<any> {
        return items.filter(item => item.costType === costType);
    }
} 

//@Pipe({
//    name: 'filterFurther'
//})
//export class FilterFurtherArrayPipe implements PipeTransform {
//    transform(items: Array<any>, type: string, owner: number): Array<any> {
//        let sub: Array<any> = items.filter(item => item.type === type);
//        return sub.filter(s => s.owner === owner);
//    }

//    //transform(items: Array<any>, type: string, owner: number): Array<any> {
//    //    let sub: Array<any> = items.filter(item => item.type === type);
//    //    var filteredItem = sub.filter(s => s.owner === owner);

//    //    var total = 0;
//    //    for (var i = 0; i < filteredItem.length; i++) {
//    //        for (var val in filteredItem[i].values) {
//    //            total += Number(filteredItem[i].values[val]);
//    //        }
//    //    }

//    //    return filteredItem.filter(item => (total != 0));

//    //}
//}

@Pipe({
    name: 'filterFurther'
})
export class FilterFurtherArrayPipe implements PipeTransform {
    
    transform(items: Array<any>, type: string, owner: number, value : string): Array<any> {
        let sub: Array<any> = items.filter(item => item.type === type);
        var filteredItem = sub.filter(s => s.owner === owner);
      
        var values = 0;
        for (var i = 0; i < filteredItem.length; i++) {
            for (var val in filteredItem[i][value]) {
                values += Number(filteredItem[i][value][val]);
            }
         
        }

        return filteredItem.filter(item => (values != 0));

    }
}

@Pipe({
    name: 'filterFurtherAllowZero'
})
export class FilterFurtherAllowZeroArrayPipe implements PipeTransform {

    transform(items: Array<any>, type: string, owner: number, value: string): Array<any> {
        let sub: Array<any> = items.filter(item => item.type === type);
        var filteredItem = sub.filter(s => s.owner === owner);

        var values = 0;
        for (var i = 0; i < filteredItem.length; i++) {
            for (var val in filteredItem[i][value]) {
                values += Number(filteredItem[i][value][val]);
            }

        }

        return filteredItem;

    }
}


@Pipe({
    name: 'filterHide'
})
export class FilteHiderArrayPipe implements PipeTransform {
    transform(items: Array<any>, category: string): Array<any> {

        var filteredItem = items.filter(item => (item.owner === category));

        var total = 0;
        for (var i = 0; i < filteredItem.length;i++)
        {
            for (var val in filteredItem[i].values) {
                total += Number(filteredItem[i].values[val]);
            }
        }

        return filteredItem.filter(item => (total != 0));
    }
}



