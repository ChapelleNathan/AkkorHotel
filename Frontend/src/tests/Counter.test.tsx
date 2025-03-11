import { render, screen, fireEvent } from '@testing-library/react';
import Counter from './Counter';

describe('Counter Component', () => {
  test('renders counter with initial value', () => {
    render(<Counter />);
    expect(screen.getByText(/Compteur: 0/i)).toBeInTheDocument();
  });

  test('increments value when increment button is clicked', () => {
    render(<Counter />);
    fireEvent.click(screen.getByText(/Incrémenter/i));
    expect(screen.getByText(/Compteur: 1/i)).toBeInTheDocument();
  });

  test('decrements value when decrement button is clicked', () => {
    render(<Counter />);
    fireEvent.click(screen.getByText(/Décrémenter/i));
    expect(screen.getByText(/Compteur: -1/i)).toBeInTheDocument();
  });
});