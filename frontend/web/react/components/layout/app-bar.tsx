import React from 'react';
import TopNavigation from '@/components/layout/top-navigation';
import AppBarLogo from '@/components/layout/app-bar-logo';

export default function AppBar() {
  return (
    <header className="flex h-14 justify-between dark:bg-neutral-900 p-1.5">

      <div className="flex items-center gap-4 pr-8">
        <button className="rounded-lg bg-blue-500 px-4 py-1.5 text-white shadow-md transition-all hover:bg-blue-400">
          Sign In
        </button>
        <button className="rounded-md bg-blue-500 px-5 py-2 text-white shadow-md transition-all hover:bg-blue-400">
          Sign Up
        </button>
      </div>
      <AppBarLogo />
      <TopNavigation />
    </header>
  );
}
