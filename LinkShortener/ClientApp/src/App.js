import React, { Component } from 'react';
import {createBrowserRouter, RouterProvider} from 'react-router-dom';
import AppRoutes from './AppRoutes';
import './custom.css';

const router = createBrowserRouter(AppRoutes);

export default class App extends Component {
  static displayName = App.name;
  
  
  render() {
    return <RouterProvider router={router} />
  }
}
