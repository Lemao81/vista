import React from 'react';
import AuthButtonPanel from '@/components/auth/auth-button-panel';
import AppBarLogo from '@/components/layout/app-bar-logo';
import TopNavigation from '@/components/layout/top-navigation';

export default function AppBar() {
  return (
    <header className="flex justify-between dark:bg-neutral-900 p-1.5">
      <AppBarLogo />
      <TopNavigation />
      <AuthButtonPanel />
    </header>
  );
}
