import { JSX, useState } from 'react';

export default function Counter(): JSX.Element {
  const [count, setCount] = useState<number>(0);

  return (
    <div>
      <h2>Compteur: {count}</h2>
      <button onClick={() => setCount(count + 1)}>Incrémenter</button>
      <button onClick={() => setCount(count - 1)}>Décrémenter</button>
    </div>
  );
}