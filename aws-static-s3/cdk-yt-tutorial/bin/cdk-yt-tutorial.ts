#!/usr/bin/env node
import * as cdk from '@aws-cdk/core';
import { CdkYtTutorialStack } from '../lib/cdk-yt-tutorial-stack';

const app = new cdk.App();
new CdkYtTutorialStack(app, 'CdkYtTutorialStack');
