import { DatePipe, formatDate } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { HRVRecordService } from '../Services/hrvrecord.service';
import { hrvOutput, hrvRecord } from '../Structures/structures';


@Component({
  selector: 'app-hrv',
  templateUrl: './hrv.component.html',
  styleUrls: ['./hrv.component.scss']
})
export class HRVComponent implements OnInit {

  public formHRV: FormGroup ; 
  public hrvs: hrvOutput[] = []; 
  public PanelEnum = Panel; 
  public panelValue: Panel; 
  public hrvDelete: hrvOutput; 
  constructor(private fb: FormBuilder, 
              private hrvService: HRVRecordService, 
              private activedRoute: ActivatedRoute, 
              private router: Router) {
    this.panelValue = Panel.Loading; 

    this.formHRV = this.fb.group({}); 

    var idString = this.activedRoute.snapshot.paramMap.get("id"); 

    if(idString == null) { 

      this.hrvService.GetHrv().subscribe(result => {
        this.hrvs = result;
        console.log(this.hrvs);
        this.panelValue = Panel.List; 
        console.log(this.panelValue);
      }, error => { 
        console.log(error); 
        if(error.status == 401) { 
          this.router.navigate(['/home']); 
        }
      });    

    }
    else { 
      if(idString === "0") {
         
        this.formHRV = fb.group({ 
          date: [new Date().toLocaleString(), [Validators.required]],
          hrv: ['', [Validators.required]], 
          id: [0, [Validators.required]]
        });

        this.panelValue = Panel.Form;

      }
      else { 
        this.hrvService.GetAHrv(parseInt(idString)).subscribe(result => {
          var lang = navigator.language;
          var pipe = new DatePipe(lang);
          var dateFormat = pipe.transform(result.date, 'medium');

          this.formHRV = fb.group({
            date: [dateFormat, [Validators.required]],
            hrv: [result.hrv, [Validators.required]],
            id: [result.id, [Validators.required]]
          });

          this.panelValue = Panel.Form;
        });
      }
    }

   }

  ngOnInit(): void {
  }

  addHrv() : void { 
    this.router.navigate(['/hrv/0']);
  }

  editHrv(id: number) : void { 
    this.router.navigate([`/hrv/${id}`]);
  }

  submit() : void { 
    var dateString = this.formHRV.get("date")?.value; 
    console.log(dateString);

    var d = new Date(Date.parse(dateString));
    console.log(d); 
    var offset = d.getTimezoneOffset();
    console.log(`offset ${offset}`); 

    // Build Date String 
    var year = d.getFullYear(); 
    var month = (d.getMonth() + 1)  < 10 ? `0${(d.getMonth() + 1)}` : (d.getMonth() + 1); 
    var day = d.getDate() < 10 ? `0${d.getDate()}` : d.getDate(); 
    var hour = d.getHours() < 10 ? `0${d.getHours()}` : d.getHours(); 
    var minute = d.getMinutes() < 10 ? `0${d.getMinutes()}` : d.getMinutes(); 
    var second = d.getSeconds() < 10 ? `0${d.getSeconds()}` : d.getSeconds();  
    var offSetHours = Math.abs(Math.floor(offset / 60)); 
    var offSetMinutes = offset % 60; 

    var offHours = offSetHours < 10 ? `0${offSetHours}` : offSetHours; 
    var offMinutes = offSetMinutes < 10 ? `0${offSetMinutes}` : offSetMinutes; 
    var sign = offset < 0 ? '+' : '-'; 
  

    var dString = `${year}-${month}-${day}T${hour}:${minute}:${second}${sign}${offHours}:${offMinutes}`; 
    var hrvReading = parseInt(this.formHRV.get("hrv")?.value); 

    var record = { date: dString, hrv: hrvReading } as hrvRecord
    var id = parseInt(this.formHRV.get("id")?.value); 
     

    if(id === 0) { 
      console.log("Adding");
      console.log(record);
      this.hrvService.AddHrv(record).subscribe(r => { 
        this.hrvs.push(r);
        this.router.navigate(['/hrv']);
      }); 
    }
    else { 
      console.log("Updating");
      console.log(record); 
      this.hrvService.UpdateHrv(record, id).subscribe(r => { 
        this.router.navigate(['/hrv']);
      })
    }
  }

  showForm(panel: Panel) : boolean { 
    if((panel & this.panelValue) === panel) return true; 
    else return false;
  }

  cancel() : void { 
    this.router.navigate(['/hrv']); 
    this.panelValue = Panel.List;
  }

  public showDeleteForm(id: number) { 
    this.hrvDelete = this.hrvs.find(h => h.id); 
    this.panelValue = this.PanelEnum.DeleteForm; 
  }

  public deleteRecord(id: number) { 
    this.panelValue = this.PanelEnum.Loading;

    this.hrvService.DeleteHRV(id).subscribe(result => { 
      console.log(result)
      var index = this.hrvs.findIndex(f => f.id == id); 
      this.hrvs.splice(index, 1); 
      this.panelValue = Panel.List;
    }, error => { 
      console.error(error);
    })
  }
}

export enum Panel { 
  Loading = 1, 
  List = 2, 
  Form = 4,
  DeleteForm = 8
}
