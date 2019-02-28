import { AppPage } from './app.po';
import { } from 'jasmine';

describe('App', () => {
  let page: AppPage;

  beforeEach(() => {
    page = new AppPage();
  });

  it('should display welcome message', () => {
    page.navigateTo();
    expect<any>(page.getMainHeading()).toEqual("Hello, world!");
 
  });
});
