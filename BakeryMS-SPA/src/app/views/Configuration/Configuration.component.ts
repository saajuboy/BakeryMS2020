import { Component, OnInit } from '@angular/core';
import { BusinessPlace } from '../../_models/businessPlace';
import { Configuration, ConfigurationList } from '../../_models/configuration';
import { AlertifyService } from '../../_services/alertify.service';
import { AuthService } from '../../_services/auth.service';
import { ConfigurationService } from '../../_services/configuration.service';
import { MasterService } from '../../_services/master.service';

@Component({
  selector: 'app-Configuration',
  templateUrl: './Configuration.component.html',
  styleUrls: ['./Configuration.component.scss']
})
export class ConfigurationComponent implements OnInit {
  businessPlaces: BusinessPlace[] = [];
  businessPlaceId: number = 0;
  configurations: Configuration[] = [];
  configToCreate: ConfigurationList = <ConfigurationList>{ configurations: [] };
  userId: number;

  constructor(private masterService: MasterService,
    private configService: ConfigurationService,
    private authService: AuthService,
    private alertify: AlertifyService) { }

  ngOnInit() {
    this.userId = this.authService.getuserId();
    this.masterService.getBusinessPlaces().subscribe(
      (res) => {
        this.businessPlaces = res;
      },
      () => {
        this.alertify.warning('some error occured');
      });

    this.configService.getConfigurations().subscribe(
      (res) => {
        this.configurations = res;
        console.log(this.configurations);
        let businessPlace = 0;
        if (this.configurations.some(x => x.description === 'BusinessPlace')) {
          businessPlace = +this.configurations.find(x => x.description === 'BusinessPlace').value;
        }
        // console.log(businessPlace);

        this.businessPlaceId = businessPlace === undefined ? 0 : +businessPlace;
      },
      () => {
        this.alertify.warning('some error occured');
      });
  }

  businessPlaceChanged() {
    if (this.configToCreate.configurations.some(a => a.description === 'BusinessPlace')) {
      this.configToCreate.configurations.find(a => a.description === 'BusinessPlace').value = this.businessPlaceId.toString();
    } else {
      this.configToCreate.configurations.push(<Configuration>{
        id: 0,
        description: 'BusinessPlace',
        value: this.businessPlaceId + '',
        userId: this.userId
      });
    }

    // console.log(this.configToCreate);

  }

  updateConfig() {
    if (this.configToCreate.configurations.length > 0) {
      this.configService.updateConfigurations(this.configToCreate).subscribe(
        () => {
          this.alertify.success('success!!');
          this.ngOnInit();
        },
        () => {
          this.alertify.error('some error occured');
        });
    }
  }

}
