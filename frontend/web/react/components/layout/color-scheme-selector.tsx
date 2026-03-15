'use client';

import { Moon, Sun, SunMoon } from 'lucide-react';

export default function ColorSchemeSelector() {
  return (
    <button
      className="flex justify-center items-center aspect-square bg-surface4 rounded-[50%] m-0.5"
      type="button"
    >
      <Sun
        className="color-scheme-light hidden"
        onClick={() => document.querySelector('html')?.setAttribute('color-scheme', 'light')}
      />
      <Moon
        className="color-scheme-dark hidden"
        onClick={() => document.querySelector('html')?.setAttribute('color-scheme', 'dark')}
      />
      <SunMoon
        className="color-scheme-auto hidden"
        onClick={() => document.querySelector('html')?.removeAttribute('color-scheme')}
      />
    </button>
  );
}
