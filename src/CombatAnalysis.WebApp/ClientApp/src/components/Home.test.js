import '@testing-library/jest-dom';
import { fireEvent, render, screen } from '@testing-library/react';
import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { MemoryRouter, useNavigate } from 'react-router-dom';
import { useLazyIdentityQuery } from '../store/api/UserApi';
import Home from './Home';

jest.mock('react-router-dom', () => ({
    ...jest.requireActual('react-router-dom'),
    useNavigate: jest.fn(),
}));

jest.mock('react-redux', () => ({
  ...jest.requireActual('react-redux'),
  useSelector: jest.fn(),
  useDispatch: jest.fn(),
}));

jest.mock('../store/api/UserApi', () => ({
  useLazyIdentityQuery: jest.fn(),
}));

jest.mock('react-i18next', () => ({
  useTranslation: () => ({
    t: (key) => key,  // Return the key as the translation
    i18n: {
      changeLanguage: jest.fn(),
    },
  }),
  initReactI18next: {
    type: '3rdParty',
    init: jest.fn(),
  },
}));

describe('Home Component', () => {
  beforeEach(() => {
    useSelector.mockReturnValue(null);
    useDispatch.mockReturnValue(jest.fn());
    useLazyIdentityQuery.mockReturnValue([jest.fn()]);
  });

  afterEach(() => {
    jest.clearAllMocks();
  });

  test('renders the component', () => {
    render(
      <MemoryRouter>
        <Home />
      </MemoryRouter>
    );

    const communityElements = screen.getAllByText('Communication');
    expect(communityElements.length).toBeGreaterThan(0);
    communityElements.forEach(element => {
      expect(element).toBeInTheDocument();
    });

    const analyzingElements = screen.getAllByText('Analyzing');
    expect(analyzingElements.length).toBeGreaterThan(0);
    analyzingElements.forEach(element => {
      expect(element).toBeInTheDocument();
    });
  });

  test('renders the authorize alert when customer is null', () => {
    useSelector.mockReturnValue(null);

    render(
      <MemoryRouter>
        <Home />
      </MemoryRouter>
    );

    expect(screen.getByText('ShouldAuthorize')).toBeInTheDocument();
  });

  test('does not render the authorize alert when customer is not null', () => {
    useSelector.mockReturnValue({});

    render(
      <MemoryRouter>
        <Home />
      </MemoryRouter>
    );

    expect(screen.queryByText('ShouldAuthorize')).toBeNull();
  });

  test('navigates to feed when "Open" button is clicked', () => {
    const navigateMock = jest.fn();
    useNavigate.mockReturnValue(navigateMock);
    useSelector.mockReturnValue({});

    render(
      <MemoryRouter>
        <Home />
      </MemoryRouter>
    );

    const openElements = screen.getAllByText('Open');
    expect(openElements.length).toBeGreaterThan(0);
    openElements.forEach(element => {
      fireEvent.click(element);
    });

    expect(navigateMock).toHaveBeenCalledWith('/feed');
  });

  test('navigates to main information when "Open" button is clicked', () => {
    const navigateMock = jest.fn();
    useNavigate.mockReturnValue(navigateMock);
    useSelector.mockReturnValue({});

    render(
      <MemoryRouter>
        <Home />
      </MemoryRouter>
    );

    const openElements = screen.getAllByText('Open');
    expect(openElements.length).toBeGreaterThan(0);
    openElements.forEach(element => {
      fireEvent.click(element);
    });

    expect(navigateMock).toHaveBeenCalledWith('/main-information');
  });
});
