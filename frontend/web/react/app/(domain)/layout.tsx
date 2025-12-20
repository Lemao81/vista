import React from 'react';
import AppBar from '@/components/layout/app-bar';
import SideNavigation from '@/components/layout/side-navigation';
import PageContainer from '@/components/layout/page-container';

export default function DomainLayout({ children }: Readonly<{ children: React.ReactNode }>) {
  return (
    <div className="flex min-h-screen flex-col">
      <AppBar />
      <div className="flex grow">
        <SideNavigation />
        <PageContainer>{children}</PageContainer>
      </div>
    </div>
  );
}
